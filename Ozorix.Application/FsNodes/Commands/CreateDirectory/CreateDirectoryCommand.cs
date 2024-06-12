using ErrorOr;
using MediatR;
using Ozorix.Domain.FsNodeAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Application.FsNodes.Commands.CreateDirectory;

public record CreateDirectoryCommand(
    string Path,
    string UserId
    ) : IRequest<ErrorOr<CreateDirectoryCommandResponse>>;
