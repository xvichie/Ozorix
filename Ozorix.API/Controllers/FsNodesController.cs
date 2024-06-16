using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ozorix.Api.Controllers;
using Ozorix.Application.FsNodes.Commands.CopyDirectory;
using Ozorix.Application.FsNodes.Commands.CopyFile;
using Ozorix.Application.FsNodes.Commands.CreateDirectory;
using Ozorix.Application.FsNodes.Commands.DeleteDirectory;
using Ozorix.Application.FsNodes.Commands.DeleteFile;
using Ozorix.Application.FsNodes.Commands.MoveDirectory;
using Ozorix.Application.FsNodes.Commands.MoveFile;
using Ozorix.Application.FsNodes.Commands.SetWorkingDirectory;
using Ozorix.Application.FsNodes.Commands.WriteFile;
using Ozorix.Application.FsNodes.Queries.GetCurrentDirectory;
using Ozorix.Application.FsNodes.Queries.GetInfo;
using Ozorix.Application.FsNodes.Queries.ListDirectory;
using Ozorix.Application.FsNodes.Queries.ReadFile;
using Ozorix.Contracts.FsNodes.CopyDirectory;
using Ozorix.Contracts.FsNodes.CopyFile;
using Ozorix.Contracts.FsNodes.CreateDirectory;
using Ozorix.Contracts.FsNodes.DeleteDirectory;
using Ozorix.Contracts.FsNodes.DeleteFile;
using Ozorix.Contracts.FsNodes.GetInfo;
using Ozorix.Contracts.FsNodes.GetWorkingDirectory;
using Ozorix.Contracts.FsNodes.ListDirectory;
using Ozorix.Contracts.FsNodes.MoveDirectory;
using Ozorix.Contracts.FsNodes.MoveFile;
using Ozorix.Contracts.FsNodes.ReadFile;
using Ozorix.Contracts.FsNodes.SetWorkingDirectory;
using Ozorix.Contracts.FsNodes.WriteFile;

namespace Ozorix.API.Controllers;

[Route("fsNodes")]
[AllowAnonymous]
public class FsNodesController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public FsNodesController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("createDirectory")]
    public async Task<IActionResult> CreateDirectory(CreateDirectoryRequest request)
    {
        var command = _mapper.Map<CreateDirectoryCommand>(request);

        var createDirectoryResult = await _mediator.Send(command);

        return createDirectoryResult.Match(
            directory => Ok(_mapper.Map<CreateDirectoryResponse>(directory)),
            errors => Problem(errors));
    }

    [HttpPost("deleteDirectory")]
    public async Task<IActionResult> DeleteDirectory(DeleteDirectoryRequest request)
    {
        var command = _mapper.Map<DeleteDirectoryCommand>(request);

        var deleteDirectoryResult = await _mediator.Send(command);

        return deleteDirectoryResult.Match(
            success => Ok(_mapper.Map<DeleteDirectoryResponse>(success)),
            errors => Problem(errors));
    }

    [HttpPost("copyDirectory")]
    public async Task<IActionResult> CopyDirectory(CopyDirectoryRequest request)
    {
        var command = _mapper.Map<CopyDirectoryCommand>(request);

        var copyDirectoryResult = await _mediator.Send(command);

        return copyDirectoryResult.Match(
            directory => Ok(_mapper.Map<CopyDirectoryResponse>(directory)),
            errors => Problem(errors));
    }

    [HttpPost("moveDirectory")]
    public async Task<IActionResult> MoveDirectory(MoveDirectoryRequest request)
    {
        var command = _mapper.Map<MoveDirectoryCommand>(request);

        var moveDirectoryResult = await _mediator.Send(command);

        return moveDirectoryResult.Match(
            directory => Ok(_mapper.Map<MoveDirectoryResponse>(directory)),
            errors => Problem(errors));
    }

    [HttpPost("listDirectory")]
    public async Task<IActionResult> ListDirectory(ListDirectoryRequest request)
    {
        var query = _mapper.Map<ListDirectoryQuery>(request);

        var listDirectoryResult = await _mediator.Send(query);

        return listDirectoryResult.Match(
            fsNodes => Ok(_mapper.Map<ListDirectoryResponse>(fsNodes)),
            errors => Problem(errors));
    }

    [HttpPost("writeFile")]
    public async Task<IActionResult> WriteFile([FromForm] WriteFileRequest request)
    {
        // Manually create the command to avoid mapping issues
        var command = new WriteFileCommand(request.Path, request.File, request.UserId);

        var writeFileResult = await _mediator.Send(command);

        return writeFileResult.Match(
            success => Ok(new WriteFileResponse(success.Success)),
            errors => Problem(errors));
    }

    [HttpPost("readFile")]
    public async Task<IActionResult> ReadFile([FromBody] ReadFileRequest request)
    {
        var query = new ReadFileQuery(request.Path, request.UserId);

        var readFileResult = await _mediator.Send(query);

        return readFileResult.Match(
            file =>
            {
                var result = File(file.Content, file.ContentType, file.FileName);
                return (IActionResult)result;
            },
            errors => Problem(errors));
    }

    [HttpPost("deleteFile")]
    public async Task<IActionResult> DeleteFile([FromBody] DeleteFileRequest request)
    {
        var command = _mapper.Map<DeleteFileCommand>(request);

        var deleteFileResult = await _mediator.Send(command);

        return deleteFileResult.Match(
            success => Ok(new DeleteFileResponse(success.Success)),
            errors => Problem(errors));
    }

    [HttpPost("copyFile")]
    public async Task<IActionResult> CopyFile([FromBody] CopyFileRequest request)
    {
        var command = _mapper.Map<CopyFileCommand>(request);

        var copyFileResult = await _mediator.Send(command);

        return copyFileResult.Match(
            response => Ok(new CopyFileResponse(response.Path)),
            errors => Problem(errors));
    }

    [HttpPost("moveFile")]
    public async Task<IActionResult> MoveFile([FromBody] MoveFileRequest request)
    {
        var command = _mapper.Map<MoveFileCommand>(request);

        var moveFileResult = await _mediator.Send(command);

        return moveFileResult.Match(
            response => Ok(new MoveFileResponse(response.Path)),
            errors => Problem(errors));
    }

    [HttpPost("getFileInfo")]
    public async Task<IActionResult> GetFileInfo([FromBody] GetInfoRequest request)
    {
        var query = _mapper.Map<GetInfoQuery>(request);

        var getInfoResult = await _mediator.Send(query);

        return getInfoResult.Match(
            info => Ok(_mapper.Map<GetInfoResponse>(info)),
            errors => Problem(errors));
    }

    [HttpPost("setWorkingDirectory")]
    public async Task<IActionResult> SetWorkingDirectory(SetWorkingDirectoryRequest request)
    {
        var command = _mapper.Map<SetWorkingDirectoryCommand>(request);
        ErrorOr<Unit> result = await _mediator.Send(command);

        return result.Match(
            _ => Ok(),
            errors => Problem(errors));
    }

    [HttpGet("getCurrentDirectory")]
    public async Task<IActionResult> GetCurrentDirectory([FromQuery] GetWorkingDirectoryRequest request)
    {
        var query = _mapper.Map<GetWorkingDirectoryQuery>(request);
        ErrorOr<string> result = await _mediator.Send(query);

        return result.Match(
            currentDirectory => Ok(new { CurrentDirectory = currentDirectory }),
            errors => Problem(errors));
    }
}
