using Domain.Common;
using Domain.Logging.Model;
using Domain.Logging.Port;

namespace Infrastructure.Persistence.Repository.Logging;

public class EfAlertRepository(ApplicationDbContext context) : GenericRepository<Alert>(context), AlertRepositoryPort
{
}
