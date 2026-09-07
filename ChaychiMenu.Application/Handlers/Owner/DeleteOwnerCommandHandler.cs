using ChaychiMenu.Application.Commands.Owner;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace ChaychiMenu.Application.Handlers.Owner;

public class DeleteOwnerCommandHandler(UserManager<Domain.Entities.AppUser> userManager) : IRequestHandler<DeleteOwnerCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(DeleteOwnerCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.AppUserId.ToString());
        if (user is null) 
            return Error.NotFound("User.NotFound", "User not found");

        user.IsDeleted = true;
        
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return result.Errors
                .Select(x => Error.Unexpected(x.Code, x.Description))
                .ToList();
        
        return user.Id;
    }
}