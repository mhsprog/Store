using API.Helper.DTOS;
using Application.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    public class ProductsController : BaseAppController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromForm] ProductParam param)
        {
            return HandlePagedResponse(await Mediator.Send(new List.Query { Params = param }));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return HandleResponse(await Mediator.Send((new Details.Query { Id = id })));
        }
    }
}
