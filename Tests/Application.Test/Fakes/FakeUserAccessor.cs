using Application.Interfaces;
using Persistence;

namespace Application.Test.Fakes;
public class FakeUserAccessor : IUserAccessor
{
    private readonly DataContext _context;

    public FakeUserAccessor()
    {
        var databaseManager = new DatabaseManager();
        _context = databaseManager.GetDbContext();
    }
    public Guid GetUserId()
    {
        _ = Guid.TryParse(_context.Users.FirstOrDefault()!.Id.ToString(), out var userId);
        return userId;
    }
}
