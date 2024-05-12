using AutoMapper;
using ReStack.Common.Models;
using ReStack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReComponent.Common.Models.Profiles
{
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
}
