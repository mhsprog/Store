using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Config;

namespace Persistence;

public class DataContext : IdentityDbContext<User,Role,Guid>
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ProductDbConfig());
        modelBuilder.ApplyConfiguration(new UserDbConfig());
    }

    public DbSet<Product> Products { get; set; }
}
