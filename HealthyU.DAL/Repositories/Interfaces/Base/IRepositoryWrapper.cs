using HealthyU.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace HealthyU.DAL.Repositories.Interfaces.Base;

public interface IRepositoryWrapper
{
    IArticleRepository ArticleRepository { get; }
    IUserRepository UserRepository { get; }
    IRecipeRepository RecipeRepository { get; }
    IImageRepository ImageRepository { get; }
    ISearchKeywordRepository SearchKeywordRepository { get; }
    IRecipeSearchKeywordRepository RecipeSearchKeywordRepository { get; }
    IRecipeNutritionRepository RecipeNutritionRepository { get; }
    IRecipeUserRatingRepository RecipeUserRatingRepository { get; }
    IRecipeTimeInfoRepository RecipeTimeInfoRepository { get; }
    IRecipeIngredientRepository RecipeIngredientRepository { get; }
    IRecipeInstructionRepository RecipeInstructionRepository { get; }
    IBMIRepository BMIRepository { get; }

    public int SaveChanges();

    public Task<int> SaveChangesAsync();

    public Task<IDbContextTransaction> BeginTransactionAsync();
}