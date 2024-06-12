using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Contracts.FsNodes.ListDirectory;

public record ListDirectoryRequest(string Path, string UserId);
