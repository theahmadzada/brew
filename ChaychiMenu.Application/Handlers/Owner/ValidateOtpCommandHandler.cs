using ChaychiMenu.Application.Commands.Owner;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;

namespace ChaychiMenu.Application.Handlers.Owner;

public class ValidateOtpCommandHandler(
    IDistributedCache cache,
    UserManager<Domain.Entities.AppUser> userManager) : IRequestHandler<ValidateOtpCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(ValidateOtpCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = $"tg_otp:{request.Otp}";
        var userId = await cache.GetStringAsync(cacheKey, cancellationToken);
        if (string.IsNullOrEmpty(userId))
            return Error.Validation("Otp.InvalidOrExpired", "Otp is wrong or expired.");

        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return Error.NotFound("User.NotFound", "User not found.");

        user.TelegramId = request.TelegramId;
        var updateResult = await userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
            return Error.Failure("User.UpdateFailed", "Could not update user.");

        await cache.RemoveAsync(cacheKey, cancellationToken);

        return true;
    }
}