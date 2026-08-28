using ErrorOr;

using MediatR;

using RMS.Application.Commands.Category;
using RMS.Application.Dto;

namespace RMS.Application.Handlers.Category;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ErrorOr<CategoryDto>>
{
    public Task<ErrorOr<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}