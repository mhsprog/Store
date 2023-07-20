namespace Domain;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime ProduceDate { get; set; }
    public string ManufacturePhone { get; set; }
    public string ManufactureEmail { get; set; }
    public bool IsAvailable { get; set; }
    public Guid? CreatorId { get; set; }
    public User Creator { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Description { get; set; }
}

