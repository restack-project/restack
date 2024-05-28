using AutoMapper;
using ReStack.Domain.Entities;

namespace ReStack.Common.Models;

public class StackIgnoreRuleProfile : Profile
{
    public StackIgnoreRuleProfile()
    {
        CreateMap<StackIgnoreRule, StackIgnoreRuleModel>()
            .ReverseMap();
    }
}
