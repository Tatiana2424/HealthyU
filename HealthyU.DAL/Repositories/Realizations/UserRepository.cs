using HealthyU.DAL.Entities;
using HealthyU.DAL.Repositories.Realizations;
using HealthyU.DAL.Repositories.Realizations.Base;

namespace HealthyU.DAL.Repositories.Interfaces
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(HealthyUDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
