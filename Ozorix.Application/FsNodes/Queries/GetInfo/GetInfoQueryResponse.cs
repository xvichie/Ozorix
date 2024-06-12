using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Application.FsNodes.Queries.GetInfo;

public record GetInfoQueryResponse(
    string Name,
    string Path,
    int Size,
    string MimeType,
    DateTime CreatedDateTime,
    DateTime UpdatedDateTime);
