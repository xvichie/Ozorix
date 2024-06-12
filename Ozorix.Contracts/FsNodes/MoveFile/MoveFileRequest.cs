using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Contracts.FsNodes.MoveFile;

public record MoveFileRequest(string Path, string NewPath, string UserId);
