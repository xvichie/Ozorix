using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Contracts.FsNodes.WriteFile;

public record WriteFileRequest(string Path, IFormFile File, string UserId);
