using Application.Core;

namespace API.Helper.DTOS;

public class ProductParam : PagingParams
{
    public DateTime ProduceFrom { get; set; }
    public DateTime ProduceTo { get; set; }
    public string ManufacturePhone { get; set; }
    public string ManufactureEmail { get; set; }
    public Guid CreatorId { get; set; }
    public bool IsAvailable { get; set; } = true;
}
