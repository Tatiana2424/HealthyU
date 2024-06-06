using HealthyU.DAL.Entities;
using HealthyU.DAL.Repositories.Realizations;
using HealthyU.DAL.Repositories.Realizations.Base;

namespace HealthyU.DAL.Repositories.Interfaces;

public class RecipeRepository : RepositoryBase<Recipe>, IRecipeRepository
{
    public RecipeRepository(HealthyUDbContext dbContext)
        : base(dbContext)
    {
    }
}
