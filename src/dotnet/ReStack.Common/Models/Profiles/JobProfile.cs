using AutoMapper;
using ReStack.Domain.Entities;
using ReStack.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReStack.Common.Models.Profiles
{
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
}
