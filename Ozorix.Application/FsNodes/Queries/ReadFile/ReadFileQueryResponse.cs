using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Application.FsNodes.Queries.ReadFile;

public record ReadFileQueryResponse(string FileName, string ContentType, byte[] Content);
