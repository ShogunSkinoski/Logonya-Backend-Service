using Domain.Account.Model;
using Domain.Account.Port;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository.Account;

public class EfUserRepository : GenericRepository<User>, UserRepositoryPort
{
    public EfUserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> ExistEmailByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = _context.Set<User>().Where(x=> x.Email == email).FirstOrDefault();
        if (user != null) return true;
        return false;
    }

    public async Task<User> GetUserByEmailAsync(string email, CancellationToken token = default)
    {
        var user = _context.Set<User>()
            .Include(x=> x.RefreshTokens)
            .Where(x => x.Email == email).FirstOrDefault();
        return user;
    }
}
