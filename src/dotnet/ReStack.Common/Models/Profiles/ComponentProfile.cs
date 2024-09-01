using AutoMapper;
using ReStack.Common.Models;
using ReStack.Domain.Entities;

namespace ReComponent.Common.Models.Profiles;

public class ComponentProfile : Profile
{
    public ComponentProfile()
    {
        CreateMap<Component, ComponentModel>()
            .ForMember(x => x.WorkspaceFolder, opts => opts.MapFrom(y => $"{y.ComponentLibrary.Slug}/components/{y.FolderName}"))
            .ForMember(x => x.WorkspaceFile, opts => opts.MapFrom(y => $"{y.ComponentLibrary.Slug}/components/{y.FolderName}/{y.FileName}"))
            .ReverseMap();
    }
}
