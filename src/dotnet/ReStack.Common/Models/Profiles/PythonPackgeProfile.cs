using AutoMapper;
using PipShell.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReStack.Common.Models.Profiles
{
    public class PythonPackgeProfile : Profile
    {
        public PythonPackgeProfile()
        {
            CreateMap<PythonPackgeModel, Package>()
                .ReverseMap();
        }
    }
}
