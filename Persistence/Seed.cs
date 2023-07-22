using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence;
public class Seed
{
    public static async Task SeedData(UserManager<User> userManager)
    {
        if (userManager.Users.Any()) return;

        var user = new User
        {
            FirstName = "Super",
            LastName = "User",
            Email = "admin@site.com",
            UserName = "admin@site.com"
        };

        await userManager.CreateAsync(user, "P@ssw0rd");
    }
}