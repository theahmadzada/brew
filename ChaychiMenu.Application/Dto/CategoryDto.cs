namespace ChaychiMenu.Application.Dto;

public record CategoryDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public int Order { get; set; }
}