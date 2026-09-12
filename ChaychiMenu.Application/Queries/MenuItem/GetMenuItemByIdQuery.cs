using ChaychiMenu.Application.Dto;

using ErrorOr;

using MediatR;

namespace ChaychiMenu.Application.Queries.MenuItem;

public record GetMenuItemByIdQuery() : IRequest<ErrorOr<MenuItemDto>>
{
    public Guid Id { get; init; }
}