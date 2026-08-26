using RMS.Domain.Entities;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Identity;

using RMS.Application.Commands.Owner;

namespace RMS.Application.Handlers.Owner;

public class ChangeOwnerPasswordCommandHandler(UserManager<AppUser> userManager) : IRequestHandler<ChangeOwnerPasswordCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(ChangeOwnerPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());
        if (user is null) return Error.NotFound("User.NotFound", "User not found");

        var result = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        if (!result.Succeeded)
            return result.Errors
                .Select(x => Error.Validation(x.Code, x.Description))
                .ToList();

        return user.Id;
    }
}