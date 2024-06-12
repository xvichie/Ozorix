using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Application.Common.Interfaces.Services;

public interface IUserCacheService
{
    void AddUser(string userId);
    void RemoveUser(string userId);
    bool IsUserCached(string userId);
    List<string> GetAllUsers();
}