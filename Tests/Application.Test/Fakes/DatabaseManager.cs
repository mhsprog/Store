using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence;
using System.Reflection;

namespace Application.Test.Fakes;
public class DatabaseManager
{
    private readonly DataContext _context;
    public DatabaseManager()
    {
        var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location.Substring(0, Assembly.GetEntryAssembly().Location.IndexOf("Tests\\")));
        var appsettingsPath = Path.Combine(path, "API", "appsettings.Development.json");

        var configuration = new ConfigurationBuilder()
            .AddJsonFile(appsettingsPath, optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var options = new DbContextOptionsBuilder<DataContext>()
            .UseSqlServer(connectionString)
            .Options;

        _context = new DataContext(options);
        _context.Database.EnsureCreated();
    }

    public DataContext GetDbContext()
    {
        return _context;
    }
}
