using Application.Products;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductController : BaseApiController
{
    [HttpPost("Create")]
    public async Task<ActionResult> Create(CreateProductDto product)
    {
        return HandleResult(await Mediator.Send(new Create.Command { Product = product }));
    }
}
