using System.Text.Json.Serialization;

namespace Application.Products;
public class GetProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime ProduceDate { get; set; }
    public string ManufacturePhone { get; set; }
    public string ManufactureEmail { get; set; }
    public bool IsAvailable { get; set; }
    public string Description { get; set; }
    public string Creator { get; set; }
    [JsonIgnore]
    public Guid CreatorId { get; set; }
    public DateTime CreatedAt { get; set; }
}
