using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Mango.Services.ProductAPI.Application__BLL_.Contracts;
using Mango.Services.ProductAPI.Application__BLL_.Exceptions;
using Mango.Services.ProductAPI.Shared.DTO;
using Microsoft.Extensions.Options;

namespace Mango.Services.ProductAPI.Application__BLL_.Services
{
  public sealed class S3StorageService : IStorageS3Service
  {
    private readonly ILogger<S3StorageService> _logger;
    private readonly S3StorageDto _s3StorageConfiguration;
    private readonly AmazonS3Client _amazonS3client;

    public S3StorageService(ILogger<S3StorageService> logger, IOptions<S3StorageDto> s3StorageConfiguration)
    {
      _logger = logger;
      _s3StorageConfiguration = s3StorageConfiguration.Value;

      // конфиг для AWS SDK
      var s3Config = new AmazonS3Config
      {
        ServiceURL = _s3StorageConfiguration.EndpointURL,
        ForcePathStyle = true
      };

      // если это локальный эмулятор — подменяем HTTP-клиент для обхода SSL
      if (_s3StorageConfiguration.EndpointURL.Contains("127.0.0.1") ||
          _s3StorageConfiguration.EndpointURL.Contains("localhost"))
      {
        var handler = new HttpClientHandler();
        // Доверяем сертификату эмулятора
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

        s3Config.HttpClientFactory = new CustomHttpClientFactory(handler);
      }

      // 3. Создаем клиент один раз, используя готовый s3Config
      _amazonS3client = new AmazonS3Client(
          _s3StorageConfiguration.AccessKey,
          _s3StorageConfiguration.SecretAccessKey,
          s3Config);
    }

    public async Task<bool> CheckBucketAsync()
    {
      try
      {
        var responseFromS3 = await _amazonS3client.ListBucketsAsync();
        if (responseFromS3 == null || responseFromS3.Buckets == null)
          return false;

        return responseFromS3.Buckets.Any(b => b.BucketName.Equals(_s3StorageConfiguration.BucketName, StringComparison.OrdinalIgnoreCase));
      }
      catch (AmazonS3Exception ex)
      {
        _logger.LogError($"Error encountered on server. Message:'{ex.Message}' when listing buckets.");
        return false;
      }
    }

    public async Task<(bool, string?, string?)> UploadFileAsync(FileDto? file)
    {
      if (!await CheckBucketAsync())
        throw new BucketUndefinedException();

      var newFileName = Guid.NewGuid().ToString();
      try
      {
        var request = new PutObjectRequest
        {
          BucketName = _s3StorageConfiguration.BucketName,
          Key = newFileName,
          InputStream = file?.FileStream,
        };

        await _amazonS3client.PutObjectAsync(request);
        _logger.LogInformation($"Successfully uploaded {request.Key} to {_s3StorageConfiguration.BucketName}.");
        return (true, newFileName, GeneratePresignedURL(request.Key, 1));
      }
      catch (AmazonS3Exception ex)
      {
        _logger.LogError($"Could not upload file to {_s3StorageConfiguration.BucketName}: '{ex.Message}'");
        return (false, null, null);
      }
    }

    public string GeneratePresignedURL(string objectKey, double duration)
    {
      string urlString = string.Empty;
      try
      {
        var request = new GetPreSignedUrlRequest()
        {
          BucketName = _s3StorageConfiguration.BucketName,
          Key = objectKey,
          Expires = DateTime.UtcNow.AddHours(duration),
        };
        urlString = _amazonS3client.GetPreSignedURL(request);
      }
      catch (AmazonS3Exception ex)
      {
        _logger.LogError($"Error:'{ex.Message}'");
      }

      return urlString;
    }

    public async Task DeletePhotoAsync(Guid? photoId)
    {
      if (photoId is null) return;
      try
      {
        var deleteObjectRequest = new DeleteObjectRequest
        {
          BucketName = _s3StorageConfiguration.BucketName,
          Key = photoId.ToString(),
        };

        _logger.LogInformation($"Deleting photo: {deleteObjectRequest.Key}");
        await _amazonS3client.DeleteObjectAsync(deleteObjectRequest);
        _logger.LogInformation($"Object: {deleteObjectRequest.Key} deleted from {deleteObjectRequest.BucketName}.");
      }
      catch (AmazonS3Exception ex)
      {
        _logger.LogError($"Error encountered on server. Message:'{ex.Message}' when deleting an object.");
      }
    }

    ~S3StorageService()
    {
      _amazonS3client.Dispose();
      _logger.LogInformation("AmazonS3 connection has been closed");
    }
  }

  public class CustomHttpClientFactory : Amazon.Runtime.HttpClientFactory
  {
    private readonly HttpClientHandler _handler;
    public CustomHttpClientFactory(HttpClientHandler handler) => _handler = handler;
    public override HttpClient CreateHttpClient(IClientConfig clientConfig) => new HttpClient(_handler);
  }
}