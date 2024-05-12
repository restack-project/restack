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
    public class ComponentParameterProfile : Profile
    {
        public ComponentParameterProfile()
        {
            CreateMap<ComponentParameter, ComponentParameterModel>()
                .ForMember(x => x.DataType, opts => opts.MapFrom(y => y.DataType.ToString()))
                .ReverseMap()
                .ForMember(x => x.DataType, opts => opts.MapFrom(y => Enum.Parse<DataType>(y.DataType)));
        }
    }
}
