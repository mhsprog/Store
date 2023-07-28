using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Security.Claims;

namespace Infrastructure;
public class IsProductOwn : IAuthorizationRequirement
{
}

public class IsProductOwnHandler : AuthorizationHandler<IsProductOwn>
{
    private readonly DataContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IsProductOwnHandler(DataContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsProductOwn requirement)
    {
        _ = Guid.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);

        if (userId == Guid.Empty) return Task.CompletedTask;

        _ = Guid.TryParse(
            _httpContextAccessor.HttpContext?.Request.RouteValues.SingleOrDefault(x => x.Key == "id").Value?.ToString(),
            out var productId);

        if (productId == Guid.Empty) return Task.CompletedTask;

        var hasProduct = _dbContext.Products
            .Any(x => x.Id == productId && x.CreatorId == userId);

        if (hasProduct) context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
