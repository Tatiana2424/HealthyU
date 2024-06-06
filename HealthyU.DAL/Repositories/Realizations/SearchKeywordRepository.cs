using HealthyU.DAL.Entities;
using HealthyU.DAL.Repositories.Realizations;
using HealthyU.DAL.Repositories.Realizations.Base;

namespace HealthyU.DAL.Repositories.Interfaces;

public class SearchKeywordRepository : RepositoryBase<SearchKeyword>, ISearchKeywordRepository
{
    public SearchKeywordRepository(HealthyUDbContext dbContext)
        : base(dbContext)
    {
    }
}
