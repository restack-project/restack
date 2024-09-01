using AutoMapper;
using PipShell.Models;

namespace ReStack.Common.Models.Profiles;

public class PythonPackgeProfile : Profile
{
    public PythonPackgeProfile()
    {
        CreateMap<PythonPackgeModel, Package>()
            .ReverseMap();
    }
}
