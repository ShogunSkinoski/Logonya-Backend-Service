using Domain.Common;
using Domain.Account.Model;

namespace Domain.Account.Port;
public interface UserRepositoryPort : IGenericRepository<User>
{
    public Task<bool> ExistEmailByEmailAsync(string email, CancellationToken cancellationToken = default);

    public Task<User> GetUserByEmailAsync(string email, CancellationToken token = default);
}
