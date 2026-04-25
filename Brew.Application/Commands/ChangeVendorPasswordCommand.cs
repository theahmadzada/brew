using ErrorOr;

using MediatR;

namespace Brew.Application.Commands;

public record ChangeVendorPasswordCommand : IRequest<ErrorOr<Guid>>
{
    public Guid Id { get; set; }
    public required string OldPassword { get; init; }
    public required string NewPassword { get; init; }
}