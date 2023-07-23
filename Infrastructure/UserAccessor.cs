using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _accessor;

        public UserAccessor(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        public Guid GetUserId()
        {
            _ = Guid.TryParse(_accessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId);
            return userId;
        }
    }
}
