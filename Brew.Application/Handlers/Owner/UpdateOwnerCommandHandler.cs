using Brew.Application.Commands.Owner;
using Brew.Application.Dto;
using Brew.Domain.Entities;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace Brew.Application.Handlers.Owner;

public class UpdateOwnerCommandHandler(UserManager<AppUser> userManager) : IRequestHandler<UpdateOwnerCommand, ErrorOr<UpdatedOwnerDto>>
{
    public async Task<ErrorOr<UpdatedOwnerDto>> Handle(UpdateOwnerCommand request, CancellationToken cancellationToken)
    {
        var owner = await userManager.FindByIdAsync(request.Id.ToString());
        if (owner is null) return Error.NotFound("User.NotFound", "User not found");

        var dto = new PatchOwnerDto()
        {
            FirstName = owner.FirstName,
            LastName = owner.LastName,
            PhoneNumber = owner.PhoneNumber,
        };
        
        request.Document.ApplyTo(dto);
        owner.FirstName = dto.FirstName;
        owner.LastName = dto.LastName;
        owner.PhoneNumber = dto.PhoneNumber;

        var result = await userManager.UpdateAsync(owner);
        if (!result.Succeeded)
            return result.Errors
                .Select(x => Error.Unexpected(x.Code, x.Description))
                .ToList();
        
        return new UpdatedOwnerDto()
        {
            Id = owner.Id,
            FirstName = owner.FirstName,
            LastName = owner.LastName,
            Email = owner.Email!,
            PhoneNumber = owner.PhoneNumber!
        };
    }
}