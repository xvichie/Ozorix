using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Application.FsNodes.Queries.ReadFile;

public record ReadFileQuery(string Path, string UserId) : IRequest<ErrorOr<ReadFileQueryResponse>>;
