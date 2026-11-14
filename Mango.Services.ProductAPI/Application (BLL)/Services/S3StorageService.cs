using Amazon.S3;
using Amazon.S3.Model;
using Mango.Services.ProductAPI.Application__BLL_.Exceptions;
using Mango.Services.ProductAPI.Shared.DTO;
using Microsoft.Extensions.Options;

namespace Mango.Services.ProductAPI.Application__BLL_.Services
{
  public class S3StorageService
  {
    private readonly ILogger<S3StorageService> _logger;
    private readonly S3StorageDto _s3StorageConfiguration;
    private readonly AmazonS3Client _amazonS3client;
    public S3StorageService(ILogger<S3StorageService> logger, IOptions<S3StorageDto> s3StorageConfiguration)
    {
      _logger = logger;
      _s3StorageConfiguration = s3StorageConfiguration.Value;
      _amazonS3client = new AmazonS3Client(_s3StorageConfiguration.AccessKey, _s3StorageConfiguration.SecretAccessKey,
        new AmazonS3Config
        {
          ServiceURL = _s3StorageConfiguration.EndpointURL,
          ForcePathStyle = true
        });
    }

    public async Task<bool> CheckBucketAsync()
    {
      var responseFromS3 = await _amazonS3client.ListBucketsAsync();
      if (responseFromS3 == null || responseFromS3.Buckets == null)
        return false;

      return responseFromS3.Buckets.Any(b => b.BucketName.Equals(_s3StorageConfiguration.BucketName, StringComparison.OrdinalIgnoreCase));
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
}
