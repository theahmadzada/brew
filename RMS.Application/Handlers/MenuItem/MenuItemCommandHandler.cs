using ErrorOr;

using MediatR;

using RMS.Application.Commands.MenuItem;
using RMS.Application.Dto;

namespace RMS.Application.Handlers.MenuItem;

public class MenuItemCommandHandler : IRequestHandler<CreateMenuItemCommand, ErrorOr<MenuItemDto>>
{
    public Task<ErrorOr<MenuItemDto>> Handle(CreateMenuItemCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}