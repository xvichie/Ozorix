using Microsoft.AspNetCore.Http;
using Ozorix.Domain.FsNodeAggregate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ozorix.Application.Common.Interfaces.Services;

public interface IFsService
{
	Task CreateDirectory(string path);
	Task DeleteDirectory(string path);
	Task CopyDirectory(string path, string newPath);
	Task MoveDirectory(string path, string newPath);
	Task<FsNode[]> ListDirectory(string path, string userId);
	Task WriteFile(string path, IFormFile file, string userId);
	Task<byte[]> ReadFile(string path);
	Task DeleteFile(string path);
	Task CopyFile(string path, string newPath);
	Task MoveFile(string path, string newPath);
	Task<FsNode> GetInfo(string path);
	Task SetWorkingDirectory(string path);
	Task<string> GetWorkingDirectory();
}
