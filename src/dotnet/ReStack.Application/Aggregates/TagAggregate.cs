using AutoMapper;
using FluentValidation;
using ReStack.Application.Aggregates.Base;
using ReStack.Common.Interfaces.Aggregates;
using ReStack.Common.Interfaces.Repositories;
using ReStack.Common.Models;
using ReStack.Domain.Entities;

namespace ReStack.Application.Aggregates;

public class TagAggregate(
    ITagRepository _repository
    , IValidator<TagModel> _validator
    , IMapper _mapper
) : BaseAggregate<TagModel, Tag, ITagRepository>(_repository, _validator, _mapper), ITagAggregate
{
}
