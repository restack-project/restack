using AutoMapper;
using ReStack.Domain.Entities;

namespace ReStack.Common.Models.Profiles;

public class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<Tag, TagModel>()
            .ReverseMap();
    }
}
