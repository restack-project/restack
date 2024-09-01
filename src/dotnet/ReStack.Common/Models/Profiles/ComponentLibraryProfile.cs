using AutoMapper;
using ReStack.Domain.Entities;
using ReStack.Domain.ValueObjects;

namespace ReStack.Common.Models.Profiles;

public class ComponentLibraryProfile : Profile
{
    public ComponentLibraryProfile()
    {
        CreateMap<ComponentLibrary, ComponentLibraryModel>()
            .ForMember(x => x.Type, opts => opts.MapFrom(y => y.Type.ToString()))
            .ReverseMap()
            .ForMember(x => x.Type, opts => opts.MapFrom(y => Enum.Parse<ProgrammingLanguage>(y.Type, true)));
    }
}
