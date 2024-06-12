using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Application.FsNodes.Queries.GetCurrentDirectory;

public record GetWorkingDirectoryQuery(string UserId) 
    : IRequest<ErrorOr<string>>;