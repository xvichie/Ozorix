using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Contracts.FsNodes.GetInfo;

public record GetInfoResponse(string Name, string Path, int Size, string MimeType, DateTime CreatedDateTime, DateTime UpdatedDateTime);
