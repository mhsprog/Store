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

    [HttpPut("Edit/{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, CreateProductDto dto)
    {
        dto.Id = id;
        return HandleResult(await Mediator.Send(new Edit.Command { Product = dto }));
    }

    [HttpDelete("Delete/{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        return HandleResult(await Mediator.Send((new Delete.Command { Id = id })));
    }
}
