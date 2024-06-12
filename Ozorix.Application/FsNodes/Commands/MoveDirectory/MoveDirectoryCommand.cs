using Amazon.Runtime.Internal;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Application.FsNodes.Commands.MoveDirectory;

public record MoveDirectoryCommand(string Path, string NewPath, string UserId)
    : IRequest<ErrorOr<MoveDirectoryCommandResponse>>;
