using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using HeyRed.Mime;
using Microsoft.AspNetCore.Http;
using Ozorix.Application.Common.Interfaces.Services;
using Ozorix.Domain.FsNodeAggregate;
using Ozorix.Domain.UserAggregate.ValueObjects;
using System.IO;

public class S3FsService : IFsService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;
    private readonly IUserCacheService _userCacheService;

    public S3FsService(IAmazonS3 s3Client, string bucketName, IUserCacheService userCacheService)
    {
        _s3Client = s3Client;
        _bucketName = bucketName;
        _userCacheService = userCacheService;
    }

    private string GetUserDirectory(string path, string userId)
    {
        var currentDirectory = _userCacheService.GetCurrentDirectory(userId);
        return $"{currentDirectory}/{path}".TrimEnd('/');
    }

    public async Task CreateDirectory(string path, string userId)
    {
        var fullPath = GetUserDirectory(path, userId);
        if (!await DirectoryExists(fullPath))
        {
            await PutObjectAsync($"{fullPath}/");
        }
    }

    public async Task DeleteDirectory(string path, string userId)
    {
        var currentDirectory = _userCacheService.GetCurrentDirectory(userId);
        var fullPath = NormalizePath(currentDirectory, path);

        if (!fullPath.EndsWith("/"))
        {
            fullPath += "/";
        }

        var objects = await ListObjectsAsync(fullPath);

        if (objects.Any())
        {
            // Add the directory marker for deletion
            var directoryMarker = new S3Object { Key = fullPath };
            objects.Add(directoryMarker);

            await DeleteObjectsAsync(objects);
        }
    }


    public async Task CopyDirectory(string path, string newPath, string userId)
    {
        var currentDirectory = _userCacheService.GetCurrentDirectory(userId);
        var fullPath = Path.Combine(currentDirectory, path).Replace("\\", "/");
        var fullNewPath = Path.Combine(currentDirectory, newPath).Replace("\\", "/");

        // Ensure paths end with a slash
        if (!fullPath.EndsWith("/"))
        {
            fullPath += "/";
        }

        if (!fullNewPath.EndsWith("/"))
        {
            fullNewPath += "/";
        }

        var objects = await ListObjectsAsync(fullPath);

        foreach (var obj in objects)
        {
            var relativePath = obj.Key.Substring(fullPath.Length);
            var newKey = Path.Combine(fullNewPath, relativePath).Replace("\\", "/");

            await CopyObjectAsync(obj.Key, newKey);
        }

        await PutObjectAsync(fullNewPath);
    }

    public async Task MoveDirectory(string path, string newPath, string userId)
    {
        await CopyDirectory(path, newPath, userId);
        await DeleteDirectory(path, userId);
    }

    public async Task<FsNode[]> ListDirectory(string path, string userId)
    {
        var fullPath = GetUserDirectory(path, userId);
        if (!fullPath.EndsWith("/"))
        {
            fullPath += "/";
        }

        var objects = await ListObjectsAsync(fullPath);
        var parsedUserId = UserId.Create(Guid.Parse(userId));

        var fsNodes = objects.Select(o =>
        {
            var mimeType = MimeTypesMap.GetMimeType(o.Key);

            return FsNode.Create(
                name: Path.GetFileName(o.Key.TrimEnd('/')),
                path: o.Key,
                size: (int)o.Size,
                mimeType: mimeType,
                userId: parsedUserId
            );
        }).ToArray();

        return fsNodes;
    }

    public async Task WriteFile(string path, IFormFile file, string userId)
    {
        var fullPath = GetUserDirectory(path, userId);
        var key = Path.Combine(fullPath, file.FileName).Replace("\\", "/");
        var metadata = new Dictionary<string, string>
        {
            { "UserId", userId }
        };

        await UploadFileAsync(key, file, metadata);
    }

    private async Task UploadFileAsync(string key, IFormFile file, Dictionary<string, string> metadata)
    {
        using (var stream = file.OpenReadStream())
        {
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = key,
                BucketName = _bucketName,
                ContentType = file.ContentType
            };

            foreach (var kvp in metadata)
            {
                uploadRequest.Metadata.Add(kvp.Key, kvp.Value);
            }

            var fileTransferUtility = new TransferUtility(_s3Client);
            await fileTransferUtility.UploadAsync(uploadRequest);
        }
    }

    public async Task<byte[]> ReadFile(string path, string userId)
    {
        var fullPath = GetUserDirectory(path, userId);
        using (var response = await _s3Client.GetObjectAsync(_bucketName, fullPath))
        using (var memoryStream = new MemoryStream())
        {
            await response.ResponseStream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }


    public async Task CopyFile(string path, string newPath, string userId)
    {
        var currentDirectory = _userCacheService.GetCurrentDirectory(userId);
        var fullPath = NormalizePath(currentDirectory, path);
        var fullNewPath = NormalizePath(currentDirectory, newPath);

        // Ensure newPath ends with a slash if it's a directory
        if (!fullNewPath.EndsWith("/"))
        {
            fullNewPath = $"{fullNewPath.TrimEnd('/')}/{Path.GetFileName(fullPath)}";
        }

        await CopyObjectAsync(fullPath, fullNewPath);
    }

    public async Task MoveFile(string path, string newPath, string userId)
    {
        await CopyFile(path, newPath, userId);
        await DeleteFile(path, userId);
    }

    private string NormalizePath(string basePath, string relativePath)
    {
        if (string.IsNullOrWhiteSpace(basePath))
        {
            throw new ArgumentException("Base path cannot be null or empty", nameof(basePath));
        }

        if (string.IsNullOrWhiteSpace(relativePath))
        {
            throw new ArgumentException("Relative path cannot be null or empty", nameof(relativePath));
        }

        // Ensure basePath ends with a slash
        if (!basePath.EndsWith("/"))
        {
            basePath += "/";
        }

        // Combine the base path and relative path
        //var combinedUri = new Uri(new Uri(basePath, UriKind.RelativeOrAbsolute), relativePath);
        return Path.Combine(basePath, relativePath).Replace("\\", "/");
    }

    // Ensure DeleteFile accepts userId
    public async Task DeleteFile(string path, string userId)
    {
        var currentDirectory = _userCacheService.GetCurrentDirectory(userId);
        var fullPath = NormalizePath(currentDirectory, path);
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = fullPath
        };
        await _s3Client.DeleteObjectAsync(deleteRequest);
    }

    public async Task<FsNode> GetInfo(string path, string userId)
    {
        var fullPath = GetUserDirectory(path, userId);
        var metadataResponse = await _s3Client.GetObjectMetadataAsync(_bucketName, fullPath);
        var mimeType = MimeTypesMap.GetMimeType(fullPath);

        return FsNode.Create(
            name: Path.GetFileName(fullPath),
            path: fullPath,
            size: (int)metadataResponse.ContentLength,
            mimeType: mimeType,
            userId: UserId.Create(Guid.Parse(userId))
        );
    }
    private async Task<bool> DirectoryExists(string path)
    {
        var listResponse = await _s3Client.ListObjectsV2Async(new ListObjectsV2Request
        {
            BucketName = _bucketName,
            Prefix = $"{path}/",
            MaxKeys = 1
        });
        return listResponse.S3Objects.Any();
    }

    private async Task<List<S3Object>> ListObjectsAsync(string prefix)
    {
        var listResponse = await _s3Client.ListObjectsV2Async(new ListObjectsV2Request
        {
            BucketName = _bucketName,
            Prefix = prefix
        });
        return listResponse.S3Objects;
    }

    private async Task DeleteObjectsAsync(IEnumerable<S3Object> objects)
    {
        if (objects == null || !objects.Any())
        {
            throw new ArgumentException("No objects to delete", nameof(objects));
        }

        var deleteRequest = new DeleteObjectsRequest
        {
            BucketName = _bucketName,
            Objects = objects.Select(o => new KeyVersion { Key = o.Key }).ToList()
        };

        try
        {
            var response = await _s3Client.DeleteObjectsAsync(deleteRequest);

            if (response.DeleteErrors.Any())
            {
                var errors = string.Join(", ", response.DeleteErrors.Select(e => $"{e.Key}: {e.Message}"));
                throw new AmazonS3Exception($"Failed to delete some objects: {errors}");
            }
        }
        catch (AmazonS3Exception ex)
        {
            // Log the exception details for troubleshooting
            Console.WriteLine($"AmazonS3Exception: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            // Log the exception details for troubleshooting
            Console.WriteLine($"Exception: {ex.Message}");
            throw;
        }
    }

    private async Task CopyObjectAsync(string sourceKey, string destinationKey)
    {
        var copyRequest = new CopyObjectRequest
        {
            SourceBucket = _bucketName,
            SourceKey = sourceKey,
            DestinationBucket = _bucketName,
            DestinationKey = destinationKey
        };
        await _s3Client.CopyObjectAsync(copyRequest);
    }

    private async Task PutObjectAsync(string key)
    {
        var putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };
        await _s3Client.PutObjectAsync(putRequest);
    }

    public async Task<bool> KeyExists(string key, string userId)
    {
        var currentDirectory = _userCacheService.GetCurrentDirectory(userId);
        var fullPath = Path.Combine(currentDirectory, key).Replace("\\", "/");

        try
        {
            await _s3Client.GetObjectMetadataAsync(_bucketName, fullPath);
            return true;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            // If the key ends with a slash, it's intended to be a folder
            if (fullPath.EndsWith("/"))
            {
                // Check if any objects exist with this prefix
                var listResponse = await _s3Client.ListObjectsV2Async(new ListObjectsV2Request
                {
                    BucketName = _bucketName,
                    Prefix = fullPath,
                    MaxKeys = 1
                });

                if (listResponse.S3Objects.Any())
                {
                    return true;
                }
            }

            return false;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
