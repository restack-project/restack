using AutoMapper;
using ReStack.Common.Models;
using ReStack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReLog.Common.Models.Profiles
{
    public class LogProfile : Profile
    {
        public LogProfile()
        {
            CreateMap<Log, LogModel>()
                .ReverseMap();
        }
    }
}
