using Brew.Application.Commands;
using Brew.Application.Dto;
using Brew.Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Brew.Application.Handlers;

public class CreateVendorCommandHandler(UserManager<AppUser> userManager) : IRequestHandler<CreateVendorCommand, ErrorOr<VendorDto>>
{
    public async Task<ErrorOr<VendorDto>> Handle(CreateVendorCommand request, CancellationToken cancellationToken)
    {
        var vendor = await userManager.FindByEmailAsync(request.Email);
        if (vendor is not null) return Error.Validation("User.Exists", "User already exists");
        var newVendor = new AppUser()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Email,
            PhoneNumber = request.PhoneNumber,
        };
        
        var password = "Temp!" + Guid.NewGuid().ToString("").Substring(0, 8);
        var result = await userManager.CreateAsync(newVendor, password);
        if (!result.Succeeded)
            return result.Errors
                .Select(x => Error.Unexpected(x.Code, x.Description))
                .ToList();

        return new VendorDto()
        {
            Id = newVendor.Id,
            FirstName = newVendor.FirstName,
            LastName = newVendor.LastName,
            Email = newVendor.Email,
            Password = password,
            PhoneNumber = request.PhoneNumber,
        };
    }
}