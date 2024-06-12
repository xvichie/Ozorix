using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3;
using Ozorix.Application.Common.Interfaces.Services;
using Ozorix.Domain.FsNodeAggregate;
using Ozorix.Domain.UserAggregate.ValueObjects;
using Microsoft.Extensions.Caching.Memory;
using HeyRed.Mime;
using Microsoft.AspNetCore.Http;

public class S3FsService : IFsService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public S3FsService(IAmazonS3 s3Client, string bucketName)
    {
        _s3Client = s3Client;
        _bucketName = bucketName;
    }
    
    public async Task CreateDirectory(string path)
    {
        if (!await DirectoryExists(path))
        {
            await PutObjectAsync($"{path}/");
        }
    }

    public async Task DeleteDirectory(string path)
    {
        var objects = await ListObjectsAsync(path);

        // Include the directory marker itself
        if (!path.EndsWith("/"))
        {
            path += "/";
        }
        var directoryMarker = new S3Object { Key = path };
        objects.Add(directoryMarker);

        if (objects.Any())
        {
            await DeleteObjectsAsync(objects);
        }
    }

    public async Task CopyDirectory(string path, string newPath)
    {
        // Ensure paths end with a slash
        if (!path.EndsWith("/"))
        {
            path += "/";
        }

        if (!newPath.EndsWith("/"))
        {
            newPath += "/";
        }

        var objects = await ListObjectsAsync(path);

        foreach (var obj in objects)
        {
            // Calculate the relative path from the source directory
            var relativePath = obj.Key.Substring(path.Length);
            // Construct the new key with the newPath + original directory name
            var newKey = $"{newPath}{path}{relativePath}";

            await CopyObjectAsync(obj.Key, newKey);
        }

        // Ensure the directory marker is copied if it exists
        await PutObjectAsync($"{newPath}{path}");
    }



    public async Task MoveDirectory(string path, string newPath)
    {
        await CopyDirectory(path, newPath);
        await DeleteDirectory(path);
    }

    public async Task<FsNode[]> ListDirectory(string path, string userId)
    {
        if (!path.EndsWith("/"))
        {
            path += "/";
        }

        var objects = await ListObjectsAsync(path);

        var parsedUserId = UserId.Create(Guid.Parse(userId)); // Parse and create UserId object

        var fsNodes = objects.Select(o =>
        {
            var mimeType = MimeTypesMap.GetMimeType(o.Key);

            return FsNode.Create(
                name: Path.GetFileName(o.Key.TrimEnd('/')),
                path: o.Key,
                size: (int)o.Size,
                mimeType: mimeType, // Determine MIME type
                userId: parsedUserId // Use the parsed UserId object,
            );
        }).ToArray();

        return fsNodes;
    }


    public async Task WriteFile(string path, IFormFile file, string userId)
    {
        var parsedUserId = UserId.Create(Guid.Parse(userId)); // Parse and create UserId object

        // Create the metadata for the file
        var metadata = new Dictionary<string, string>
    {
        { "UserId", parsedUserId.Value.ToString() }
    };

        // Combine the path and the file name to create the full key
        var key = Path.Combine(path.Trim('/'), file.FileName);

        await UploadFileAsync(key.Replace("\\", "/"), file, metadata); // Ensure the key uses forward slashes
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
                ContentType = file.ContentType // Use the file's content type
            };

            // Add metadata
            foreach (var kvp in metadata)
            {
                uploadRequest.Metadata.Add(kvp.Key, kvp.Value);
            }

            var fileTransferUtility = new TransferUtility(_s3Client);
            await fileTransferUtility.UploadAsync(uploadRequest);
        }
    }



    public async Task<byte[]> ReadFile(string path)
    {
        using (var response = await _s3Client.GetObjectAsync(_bucketName, path))
        using (var memoryStream = new MemoryStream())
        {
            await response.ResponseStream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }

    public async Task DeleteFile(string path)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = path
        };
        await _s3Client.DeleteObjectAsync(deleteRequest);
    }

    public async Task CopyFile(string path, string newPath)
    {
        // Check if newPath is a directory by checking if it ends with a slash
        if (!newPath.EndsWith("/"))
        {
            newPath += "/";
        }

        // Combine newPath with the file name from path
        var fileName = System.IO.Path.GetFileName(path);
        var destinationKey = newPath + fileName;

        var copyRequest = new CopyObjectRequest
        {
            SourceBucket = _bucketName,
            SourceKey = path,
            DestinationBucket = _bucketName,
            DestinationKey = destinationKey
        };
        await _s3Client.CopyObjectAsync(copyRequest);
    }

    public async Task MoveFile(string path, string newPath)
    {
        await CopyFile(path, newPath);
        await DeleteFile(path);
    }

    public async Task<FsNode> GetInfo(string path)
    {
        var metadataResponse = await _s3Client.GetObjectMetadataAsync(_bucketName, path);
        var mimeType = MimeTypesMap.GetMimeType(path);

        return FsNode.Create(
            name: System.IO.Path.GetFileName(path),
            path: path,
            size: (int)metadataResponse.ContentLength,
            mimeType: mimeType,
            userId: UserId.CreateUnique() // Adjust as needed
            //createdDateTime: metadataResponse.LastModified,
            //updatedDateTime: metadataResponse.LastModified
        );
    }

    public Task SetWorkingDirectory(string path)
    {
        // S3 does not have a concept of a working directory, this would be client-side state management
        throw new NotImplementedException();
    }

    public Task<string> GetWorkingDirectory()
    {
        // S3 does not have a concept of a working directory, this would be client-side state management
        throw new NotImplementedException();
    }

    private async Task<bool> DirectoryExists(string path)
    {
        var listResponse = await _s3Client.ListObjectsV2Async(new ListObjectsV2Request
        {
            BucketName = _bucketName,
            Prefix = $"{path}/",
            MaxKeys = 1 // We only need to check if at least one object exists
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
            throw new ArgumentException("No objects to delete");
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
                throw new Exception($"Failed to delete some objects: {errors}");
            }
        }
        catch (AmazonS3Exception ex)
        {
            // Log the exception details for troubleshooting
            Console.WriteLine($"AmazonS3Exception: {ex.Message}");
            Console.WriteLine($"Error Code: {ex.ErrorCode}");
            Console.WriteLine($"Request ID: {ex.RequestId}");
            Console.WriteLine($"Status Code: {ex.StatusCode}");
            throw;
        }
    }

    private async Task UploadFileAsync(string key, byte[] content)
    {
        using (var stream = new MemoryStream(content))
        {
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = key,
                BucketName = _bucketName
            };
            var fileTransferUtility = new TransferUtility(_s3Client);
            await fileTransferUtility.UploadAsync(uploadRequest);
        }
    }

    private async Task<string> GetObjectContentAsync(string key)
    {
        var response = await _s3Client.GetObjectAsync(_bucketName, key);
        using (var reader = new StreamReader(response.ResponseStream))
        {
            return await reader.ReadToEndAsync();
        }
    }

    private async Task DeleteObjectAsync(string key)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };
        await _s3Client.DeleteObjectAsync(deleteRequest);
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

    private async Task<GetObjectMetadataResponse> GetObjectMetadataAsync(string key)
    {
        return await _s3Client.GetObjectMetadataAsync(_bucketName, key);
    }

    public Task<string> ReadFile(string path, byte[] content)
    {
        throw new NotImplementedException();
    }

    public Task DeleteFile()
    {
        throw new NotImplementedException();
    }
}
