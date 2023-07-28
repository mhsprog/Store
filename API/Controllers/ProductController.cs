using Application.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductController : BaseAppController
{
    [HttpPost("Create")]
    public async Task<ActionResult> Create(CreateProductDto product)
    {
        return HandleResponse(await Mediator.Send(new Create.Command { Product = product }));
    }

    [Authorize(Policy = "IsProductOwn")]
    [HttpPut("Edit/{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, CreateProductDto dto)
    {
        dto.Id = id;
        return HandleResponse(await Mediator.Send(new Edit.Command { Product = dto }));
    }

    [Authorize(Policy = "IsProductOwn")]
    [HttpDelete("Delete/{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        return HandleResponse(await Mediator.Send((new Delete.Command { Id = id })));
    }
}
