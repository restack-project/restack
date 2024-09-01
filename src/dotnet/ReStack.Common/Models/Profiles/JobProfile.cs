using AutoMapper;
using ReStack.Domain.Entities;
using ReStack.Domain.ValueObjects;

namespace ReStack.Common.Models.Profiles;

public class JobProfile : Profile
{
    public JobProfile()
    {
        CreateMap<Job, JobModel>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .ForMember(x => x.State, opts => opts.MapFrom(y => y.State.ToString()))
            .ReverseMap()
            .ForMember(x => x.State, opts => opts.MapFrom(y => Enum.Parse<JobState>(y.State)));
    }
}
