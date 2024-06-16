namespace Ozorix.Application.Common.Interfaces.Services;

public interface IUserCacheService
{
    void AddUser(string userId);
    void RemoveUser(string userId);
    bool IsUserCached(string userId);
    string GetCurrentDirectory(string userId);
    void SetCurrentDirectory(string userId, string currentDirectory);
    List<string> GetAllUsers();
}
