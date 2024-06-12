namespace Ozorix.API.Common.Mapping;

using Mapster;
using Ozorix.Application.FsNodes.Commands.CreateDirectory;
using Ozorix.Contracts.FsNodes.CreateDirectory;

using Mapster;
using Ozorix.Contracts.FsNodes.DeleteDirectory;
using Ozorix.Application.FsNodes.Commands.DeleteDirectory;
using Ozorix.Application.FsNodes.Commands.CopyDirectory;
using Ozorix.Contracts.FsNodes.CopyDirectory;
using Ozorix.Contracts.FsNodes.MoveDirectory;
using Ozorix.Application.FsNodes.Commands.MoveDirectory;
using Ozorix.Application.FsNodes.Queries.ListDirectory;
using Ozorix.Contracts.FsNodes.ListDirectory;
using Ozorix.Domain.FsNodeAggregate;
using Ozorix.Application.FsNodes.Dto;
using Ozorix.Application.FsNodes.Commands.WriteFile;
using Ozorix.Contracts.FsNodes.WriteFile;
using Ozorix.Application.FsNodes.Commands.DeleteFile;
using Ozorix.Application.FsNodes.Queries.ReadFile;
using Ozorix.Contracts.FsNodes.DeleteFile;
using Ozorix.Contracts.FsNodes.ReadFile;
using Ozorix.Application.FsNodes.Commands.CopyFile;
using Ozorix.Contracts.FsNodes.CopyFile;
using Ozorix.Application.FsNodes.Commands.MoveFile;
using Ozorix.Contracts.FsNodes.MoveFile;
using Ozorix.Application.FsNodes.Queries.GetInfo;
using Ozorix.Contracts.FsNodes.GetInfo;

public class FsNodesMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateDirectoryCommandResponse, CreateDirectoryResponse>();

        config.NewConfig<DeleteDirectoryCommandResponse, DeleteDirectoryResponse>();

        config.NewConfig<CopyDirectoryCommandResponse, CopyDirectoryResponse>();

        config.NewConfig<MoveDirectoryCommandResponse, MoveDirectoryResponse>();

        config.NewConfig<WriteFileRequest, WriteFileCommand>()
              .Ignore(dest => dest.File);

        config.NewConfig<ReadFileRequest, ReadFileQuery>();
        config.NewConfig<ReadFileQueryResponse, ReadFileResponse>();

        config.NewConfig<DeleteFileRequest, DeleteFileCommand>();
        config.NewConfig<DeleteFileCommandResponse, DeleteFileResponse>();

        config.NewConfig<CopyFileRequest, CopyFileCommand>();
        config.NewConfig<CopyFileCommandResponse, CopyFileResponse>();

        config.NewConfig<MoveFileRequest, MoveFileCommand>();
        config.NewConfig<MoveFileCommandResponse, MoveFileResponse>();

        config.NewConfig<GetInfoRequest, GetInfoQuery>();
        config.NewConfig<GetInfoQueryResponse, GetInfoResponse>();

        config.NewConfig<FsNode, FsNodeDto>()
              .Map(dest => dest.Id, src => src.Id.Value.ToString())
              .Map(dest => dest.Name, src => src.Name)
              .Map(dest => dest.Path, src => src.Path)
              .Map(dest => dest.Size, src => src.Size)
              .Map(dest => dest.MimeType, src => src.MimeType)
              .Map(dest => dest.UserId, src => src.UserId.Value.ToString());

        config.NewConfig<IEnumerable<FsNode>, ListDirectoryQueryResponse>()
              .Map(dest => dest.FsNodes, src => src);

        config.NewConfig<ListDirectoryQueryResponse, ListDirectoryResponse>()
              .Map(dest => dest.FsNodes, src => src.FsNodes.Select(f => f.Adapt<FsNodeDto>()));
    }
}