using ChaychiMenu.Application.Commands.Owner;
using ChaychiMenu.Application.Dto;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;

namespace ChaychiMenu.Application.Handlers.Owner;

public class GenerateTelegramOtpCommandHandler(
    IDistributedCache cache,
    UserManager<Domain.Entities.AppUser> userManager) : IRequestHandler<GenerateTelegramOtpCommand, ErrorOr<OtpDto>>
{
    public async Task<ErrorOr<OtpDto>> Handle(GenerateTelegramOtpCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.AppUserId.ToString());
        if (user is null)
            return Error.NotFound("User.NotFound", "Target user was not found.");

        var otpCode = Random.Shared.Next(100000, 999999).ToString();
        var cacheKey = $"tg_otp:{otpCode}";
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };

        await cache.SetStringAsync(cacheKey, request.AppUserId.ToString(), options, cancellationToken);

        return new OtpDto()
        {
            ExpiresInMinutes = 5,
            Otp = otpCode,
            AppUserId = request.AppUserId
        };
    }
}