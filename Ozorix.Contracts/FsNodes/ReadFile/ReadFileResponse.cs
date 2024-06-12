using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Contracts.FsNodes.ReadFile;

public record ReadFileResponse(string FileName, string ContentType, byte[] Content);
