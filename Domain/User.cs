using Microsoft.AspNetCore.Identity;

namespace Domain;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<Product> Products { get; set; }
}
