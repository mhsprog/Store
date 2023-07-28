using API.Extensions;
using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseAppController : ControllerBase
    {

        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected ActionResult HandleResponse<T>(Result<T> result)
        {
            if (result == null) return NotFound();
            return result.IsSuccess switch
            {
                true when result.Value != null => Ok(result.Value),
                true when result.Value == null => NotFound(),
                _ => BadRequest(result.Message)
            };
        }

        protected ActionResult HandlePagedResponse<T>(Result<PagedList<T>> result)
        {
            switch (result)
            {
                case null:
                    return NotFound();

                case { IsSuccess: true, Value: { } }:
                    Response.AddPaginationHeader(result.Value.CurrentPage, result.Value.PageSize,
                        result.Value.TotalCount, result.Value.TotalPage);
                    return Ok(result.Value);

                case { IsSuccess: true, Value: null }:
                    return NotFound();

                default:
                    return BadRequest(result.Message);
            }
        }
    }
}
