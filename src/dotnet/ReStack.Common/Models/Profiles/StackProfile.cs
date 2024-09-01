using AutoMapper;
using ReStack.Domain.Entities;
using ReStack.Domain.ValueObjects;

namespace ReStack.Common.Models.Profiles;

public class StackProfile : Profile
{
    public StackProfile()
    {
        CreateMap<Stack, StackModel>()
            .ForMember(x => x.Type, opts => opts.MapFrom(y => y.Type.ToString()))
            .ForMember(x => x.FileName, opts => opts.MapFrom(y => y.GetFileName()))
            .ForMember(x => x.AverageRuntime, opts => opts.MapFrom(y => TimeSpan.FromSeconds((double)y.AverageRuntime)))
            .ForMember(x => x.Components, opts => opts.MapFrom(y => y.Components.Select(x => x.Component).ToList()))
            .ForMember(x => x.Tags, opts => opts.MapFrom(y => y.Tags.Select(x => new TagModel(x)).ToList()))
            .ReverseMap()
            .ForMember(x => x.Type, opts => opts.MapFrom(y => Enum.Parse<ProgrammingLanguage>(y.Type, true)))
            .ForMember(x => x.AverageRuntime, opts => opts.MapFrom(y => y.AverageRuntime.TotalSeconds))
            .ForMember(x => x.Components, opts => opts.MapFrom(y => y.Components.Select(z => new StackComponent() { ComponentId = z.Id, StackId = y.Id }).ToList()))
            .ForMember(x => x.Tags, opts => opts.MapFrom(y => y.Tags.Select(z => new StackTag() { TagId = z.Id, StackId = y.Id }).ToList()));
    }
}
