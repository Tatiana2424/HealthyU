using HealthyU.DAL.Entities;
using HealthyU.DAL.Repositories.Interfaces;
using HealthyU.DAL.Repositories.Realizations.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyU.DAL.Repositories.Realizations;

public class RecipeSearchKeywordRepository : RepositoryBase<RecipeSearchKeyword>, IRecipeSearchKeywordRepository
{
    public RecipeSearchKeywordRepository(HealthyUDbContext dbContext)
        : base(dbContext)
    {
    }
}
