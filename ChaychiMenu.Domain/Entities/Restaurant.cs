namespace ChaychiMenu.Domain.Entities;

public class Restaurant
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public List<Category> Categories { get; set; } = [];
    //public List<Table> Tables { get; set; } = [];
    //public List<Employee> Employees { get; set; } = [];
    
    public Guid OwnerId { get; set; }
    public Owner Owner { get; set; } = null!;
    
    public Guid? ChainId { get; set; }
    public Chain? Chain { get; set; }
    public long? TelegramChatId { get; set; }
}