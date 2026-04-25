using Brew.Application.Commands;
using Brew.Application.Dto;
using Brew.Domain.Entities;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace Brew.Application.Handlers;

public class UpdateVendorCommandHandler(UserManager<AppUser> userManager) : IRequestHandler<UpdateVendorCommand, ErrorOr<UpdatedVendorDto>>
{
    public async Task<ErrorOr<UpdatedVendorDto>> Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
    {
        var vendor = await userManager.FindByIdAsync(request.Id.ToString());
        if (vendor is null) return Error.NotFound("User.NotFound", "User not found");

        var dto = new UpdateVendorDto()
        {
            FirstName = vendor.FirstName,
            LastName = vendor.LastName,
            PhoneNumber = vendor.PhoneNumber,
        };
        
        request.Document.ApplyTo(dto);
        vendor.FirstName = dto.FirstName;
        vendor.LastName = dto.LastName;
        vendor.PhoneNumber = dto.PhoneNumber;

        var result = await userManager.UpdateAsync(vendor);
        if (!result.Succeeded)
            return result.Errors
                .Select(x => Error.Unexpected(x.Code, x.Description))
                .ToList();
        
        return new UpdatedVendorDto()
        {
            Id = vendor.Id,
            FirstName = vendor.FirstName,
            LastName = vendor.LastName,
            Email = vendor.Email!,
            PhoneNumber = vendor.PhoneNumber!
        };
    }
}