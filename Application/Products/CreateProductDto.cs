using AutoMapper.Configuration.Annotations;

namespace Application.Products;
public class CreateProductDto
{
    [Ignore]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime ProduceDate { get; set; }
    public string ManufacturePhone { get; set; }
    public string ManufactureEmail { get; set; }
    public bool IsAvailable { get; set; }
    public string Description { get; set; }
}
