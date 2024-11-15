using Domain.Logging.Model;
using Domain.Logging.Port;

namespace Infrastructure.Persistence.Repository.Logging;

public class EfLogRepository(ApplicationDbContext context) : GenericRepository<Log>(context), LogRepositoryPort;

