using AutoMapper;
using ReStack.Common.Models;
using ReStack.Domain.Entities;

namespace ReLog.Common.Models.Profiles;

public class LogProfile : Profile
{
    public LogProfile()
    {
        CreateMap<Log, LogModel>()
            .ReverseMap();
    }
}
