using RMS.Domain;
using RMS.Infrastructure.DbContext;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Identity;

using RMS.Application.Commands.Owner;
using RMS.Application.Dto;

namespace RMS.Application.Handlers.Owner;

public class CreateOwnerCommandHandler(
    UserManager<Domain.Entities.AppUser> userManager,
    AppDbContext dbContext) : IRequestHandler<CreateOwnerCommand, ErrorOr<OwnerDto>>
{
    public async Task<ErrorOr<OwnerDto>> Handle(CreateOwnerCommand request, CancellationToken cancellationToken)
    {
        var appUser = await userManager.FindByEmailAsync(request.Email);
        if (appUser is not null) return Error.Validation("User.Exists", "User already exists");
       
        var newUser = new Domain.Entities.AppUser()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Email,
            PhoneNumber = request.PhoneNumber,
        };
        var password = "Temp!" + Guid.NewGuid().ToString("").Substring(0, 8);

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await userManager.CreateAsync(newUser, password);
            if (!result.Succeeded)
                return result.Errors
                    .Select(x => Error.Validation(x.Code, x.Description))
                    .ToList();

            var roleResult = await userManager.AddToRoleAsync(newUser, UserRole.Owner);
            if (!roleResult.Succeeded)
                return roleResult.Errors
                    .Select(x => Error.Unexpected(x.Code, x.Description))
                    .ToList();
            
            var owner = new Domain.Entities.Owner() { AppUserId = newUser.Id, AppUser = newUser };
            
            dbContext.Owners.Add(owner);
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        
        return new OwnerDto()
        {
            Id = newUser.Id,
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            Email = newUser.Email,
            Password = password,
            PhoneNumber = request.PhoneNumber,
        };
    }
}