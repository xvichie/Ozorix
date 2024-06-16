using Microsoft.AspNetCore.Http;
using Ozorix.Domain.FsNodeAggregate;

public interface IFsService
{
    Task CreateDirectory(string path, string userId);
    Task DeleteDirectory(string path, string userId);
    Task CopyDirectory(string path, string newPath, string userId);
    Task MoveDirectory(string path, string newPath, string userId);
    Task<FsNode[]> ListDirectory(string path, string userId);
    Task WriteFile(string path, IFormFile file, string userId);
    Task<byte[]> ReadFile(string path, string userId);
    Task DeleteFile(string path, string userId);
    Task CopyFile(string path, string newPath, string userId);
    Task MoveFile(string path, string newPath, string userId);
    Task<FsNode> GetInfo(string path, string userId);
    Task<bool> KeyExists(string key, string userId);

}
