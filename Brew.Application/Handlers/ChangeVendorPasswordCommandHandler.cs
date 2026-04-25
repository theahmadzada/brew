using Brew.Application.Commands;
using Brew.Domain.Entities;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace Brew.Application.Handlers;

public class ChangeVendorPasswordCommandHandler(UserManager<AppUser> userManager) : IRequestHandler<ChangeVendorPasswordCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(ChangeVendorPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());
        if (user is null) return Error.NotFound("User.NotFound", "User not found");

        var result = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        if (!result.Succeeded)
            return result.Errors
                .Select(x => Error.Unexpected(x.Code, x.Description))
                .ToList();

        return user.Id;
    }
}