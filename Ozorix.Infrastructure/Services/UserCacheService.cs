using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Caching.Memory;
using Ozorix.Application.Common.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Infrastructure.Services;

public class UserCacheService(IMemoryCache cache) : IUserCacheService
{
    private const string UserCacheKey = "UserCacheKey";

    public void AddUser(string userId)
    {
        var users = GetAllUsers();
        if (!users.Contains(userId))
        {
            users.Add(userId);
            cache.Set(UserCacheKey, users);
        }
    }

    public void RemoveUser(string userId)
    {
        var users = GetAllUsers();
        if (users.Contains(userId))
        {
            users.Remove(userId);
            cache.Set(UserCacheKey, users);
        }
    }

    public bool IsUserCached(string userId)
    {
        var users = GetAllUsers();
        return users.Contains(userId);
    }

    public List<string> GetAllUsers()
    {
        if (!cache.TryGetValue(UserCacheKey, out List<string> users))
        {
            users = new List<string>();
            cache.Set(UserCacheKey, users);
        }
        return users;
    }
}
