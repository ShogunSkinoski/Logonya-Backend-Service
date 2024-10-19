using Domain.Account.Model;
using Domain.Account.Port;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository.Account;

public class EfUserRepository(DbContext context) : GenericRepository<User>(context), UserRepositoryPort
{
}
