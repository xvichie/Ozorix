using ErrorOr;
using MediatR;
using Microsoft.Extensions.Hosting;
using Ozorix.Application.Common.Interfaces.Persistence;
using Ozorix.Application.Common.Interfaces.Services;
using Ozorix.Domain.FsNodeAggregate;
using Ozorix.Domain.FsNodeAggregate.ValueObjects;
using Ozorix.Domain.UserAggregate.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ozorix.Application.FsNodes.Commands.CreateFsNode;

public class CreateFsNodeCommandHandler : IRequestHandler<CreateFsNodeCommand, ErrorOr<FsNode>>
{
    private readonly IFsNodeRepository _fsNodeRepository;
    private readonly IFsService _s3FsService;
    public CreateFsNodeCommandHandler(IFsNodeRepository fsNodeRepository, IFsService s3FsService)
    {
        _fsNodeRepository = fsNodeRepository;
        _s3FsService = s3FsService;

    }

    public async Task<ErrorOr<FsNode>> Handle(CreateFsNodeCommand request, CancellationToken cancellationToken)
    {
        var fsNode = FsNode.Create(
            request.Name,
            request.Path,
            0,
            request.Path,
            UserId.Create(request.UserId)
            );

        await _s3FsService.CreateDirectory(request.Path, request.UserId.ToString());

        return fsNode;
    }
}
