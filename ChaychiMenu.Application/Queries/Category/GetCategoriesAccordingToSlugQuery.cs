using ChaychiMenu.Application.Dto;

using ErrorOr;

using MediatR;

namespace ChaychiMenu.Application.Queries.Category;

public record GetCategoriesAccordingToSlugQuery : IRequest<ErrorOr<List<GetCategoriesAccordintToSlugDto>>>
{
    public required string Slug { get; init; }
}