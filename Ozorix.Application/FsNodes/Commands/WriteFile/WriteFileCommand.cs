using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Application.FsNodes.Commands.WriteFile;

public record WriteFileCommand(string Path, IFormFile File, string UserId) 
    : IRequest<ErrorOr<WriteFileCommandResponse>>;
