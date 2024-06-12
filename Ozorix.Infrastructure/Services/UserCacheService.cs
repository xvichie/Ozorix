using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Caching.Memory;
using Ozorix.Application.Common.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Infrastructure.Services;

public class UserCacheService(IMemoryCache MemoryCache) : IUserCacheService
{
    private const string UserCacheKey = "UserCache";
    private readonly Dictionary<string, string> _userDirectories = new();

    public void AddUser(string userId)
    {
        var users = MemoryCache.Get<List<string>>(UserCacheKey) ?? new List<string>();
        if (!users.Contains(userId))
        {
            users.Add(userId);
            MemoryCache.Set(UserCacheKey, users);
        }
    }

    public void RemoveUser(string userId)
    {
        var users = MemoryCache.Get<List<string>>(UserCacheKey) ?? new List<string>();
        if (users.Contains(userId))
        {
            users.Remove(userId);
            MemoryCache.Set(UserCacheKey, users);
        }

        if (_userDirectories.ContainsKey(userId))
        {
            _userDirectories.Remove(userId);
        }
    }

    public bool IsUserCached(string userId)
    {
        var users = MemoryCache.Get<List<string>>(UserCacheKey) ?? new List<string>();
        return users.Contains(userId);
    }

    public string GetCurrentDirectory(string userId)
    {
        _userDirectories.TryGetValue(userId, out var currentDirectory);
        return currentDirectory ?? userId;
    }

    public void SetCurrentDirectory(string userId, string currentDirectory)
    {
        if (!currentDirectory.StartsWith(userId))
        {
            currentDirectory = $"{userId}/{currentDirectory}".TrimEnd('/');
        }
        _userDirectories[userId] = currentDirectory;
    }

    public List<string> GetAllUsers()
    {
        return MemoryCache.Get<List<string>>(UserCacheKey) ?? new List<string>();
    }
}
