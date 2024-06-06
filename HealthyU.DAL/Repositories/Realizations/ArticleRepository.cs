using HealthyU.DAL.Entities;
using HealthyU.DAL.Repositories.Realizations;
using HealthyU.DAL.Repositories.Realizations.Base;

namespace HealthyU.DAL.Repositories.Interfaces;

public class ArticleRepository : RepositoryBase<Article>, IArticleRepository
{
    public ArticleRepository(HealthyUDbContext dbContext)
        : base(dbContext)
    {
    }
}
