using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence;
public class Seed
{
    public static async Task SeedData(DataContext context, UserManager<User> userManager)
    {
        if (!userManager.Users.Any() && !context.Products.Any())
        {
            var users = new List<User>
            {
                new User{
                    FirstName = "Super",
                    LastName = "User",
                    Email = "admin@site.com",
                    UserName = "admin@site.com"
                },
                new User
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "jdoe@site.com",
                    UserName = "jdoe@site.com"
                }
            };


            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }

            var products = _getProductList(users.Select(x => x.Id).ToList());

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();

        }
    }

    private static List<Product> _getProductList(List<Guid> userIds)
    {
        Random random = new();
        var products = new List<Product>();

        for (int i = 1; i <= 30; i++)
        {
            products.Add(new Product
            {
                Id = Guid.NewGuid(),
                Name = $"Product {i}",
                ProduceDate = DateTime.Now.AddDays(-random.Next(40, 99)),
                ManufacturePhone = $"09{random.Next(100000000, 999999999)}",
                ManufactureEmail = $"manufacturer{i}@example.com",
                IsAvailable = i % 6 != 0,
                CreatorId = userIds[i % 2],
                CreatedAt = DateTime.Now.AddDays(i - 30),
                Description = $"This is Product {i} description."
            });
        }
        return products;
    }
}