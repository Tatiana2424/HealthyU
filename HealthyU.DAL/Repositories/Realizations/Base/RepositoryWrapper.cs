


using HealthyU.DAL.Repositories.Interfaces;
using HealthyU.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore.Storage;

namespace HealthyU.DAL.Repositories.Realizations.Base;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly HealthyUDbContext _healthUShopDbContext;

    private IArticleRepository _articleRepository;

    private IRecipeRepository _recipeRepository;

    private ISearchKeywordRepository _searchKeywordRepository;

    private IRecipeSearchKeywordRepository _recipeSearchKeywordRepository;

    private IRecipeUserRatingRepository _recipeUserRatingRepository;

    private IRecipeNutritionRepository _recipeNutritionRepository;

    private IUserRepository _userRepository;

    private IImageRepository _imageRepository;

    private IRecipeTimeInfoRepository _recipeTimeInfoRepository;

    private IRecipeIngredientRepository _recipeIngredientRepository;

    private IRecipeInstructionRepository _recipeInstructionRepository;

    private IBMIRepository _bmiRepository;

    public RepositoryWrapper(HealthyUDbContext healthUShopDbContext)
    {
        _healthUShopDbContext = healthUShopDbContext;
    }

    public IArticleRepository ArticleRepository
    {
        get
        {
            if (_articleRepository is null)
            {
                _articleRepository = new ArticleRepository(_healthUShopDbContext);
            }
            return _articleRepository;
        }
    }

    public IBMIRepository BMIRepository
    {
        get
        {
            if (_bmiRepository is null)
            {
                _bmiRepository = new BMIRepository(_healthUShopDbContext);
            }
            return _bmiRepository;
        }
    }

    public IImageRepository ImageRepository
    {
        get
        {
            if (_imageRepository is null)
            {
                _imageRepository = new ImageRepository(_healthUShopDbContext);
            }
            return _imageRepository;
        }
    }

    public IRecipeRepository RecipeRepository
    {
        get
        {
            if (_recipeRepository is null)
            {
                _recipeRepository = new RecipeRepository(_healthUShopDbContext);
            }
            return _recipeRepository;
        }
    }

    public IRecipeNutritionRepository RecipeNutritionRepository
    {
        get
        {
            if (_recipeNutritionRepository is null)
            {
                _recipeNutritionRepository = new RecipeNutritionRepository(_healthUShopDbContext);
            }
            return _recipeNutritionRepository;
        }
    }

    public IRecipeIngredientRepository RecipeIngredientRepository
    {
        get
        {
            if (_recipeIngredientRepository is null)
            {
                _recipeIngredientRepository = new RecipeIngredientRepository(_healthUShopDbContext);
            }
            return _recipeIngredientRepository;
        }
    }

    public IRecipeInstructionRepository RecipeInstructionRepository
    {
        get
        {
            if (_recipeInstructionRepository is null)
            {
                _recipeInstructionRepository = new RecipeInstructionRepository(_healthUShopDbContext);
            }
            return _recipeInstructionRepository;
        }
    }

    public IRecipeUserRatingRepository RecipeUserRatingRepository
    {
        get
        {
            if (_recipeUserRatingRepository is null)
            {
                _recipeUserRatingRepository = new RecipeUserRatingRepository(_healthUShopDbContext);
            }
            return _recipeUserRatingRepository;
        }
    }

    public IRecipeTimeInfoRepository RecipeTimeInfoRepository
    {
        get
        {
            if (_recipeTimeInfoRepository is null)
            {
                _recipeTimeInfoRepository = new RecipeTimeInfoRepository(_healthUShopDbContext);
            }
            return _recipeTimeInfoRepository;
        }
    }

    public ISearchKeywordRepository SearchKeywordRepository
    {
        get
        {
            if (_searchKeywordRepository is null)
            {
                _searchKeywordRepository = new SearchKeywordRepository(_healthUShopDbContext);
            }
            return _searchKeywordRepository;
        }
    }

    public IRecipeSearchKeywordRepository RecipeSearchKeywordRepository
    {
        get
        {
            if (_recipeSearchKeywordRepository is null)
            {
                _recipeSearchKeywordRepository = new RecipeSearchKeywordRepository(_healthUShopDbContext);
            }
            return _recipeSearchKeywordRepository;
        }
    }

    public IUserRepository UserRepository
    {
        get
        {
            if (_userRepository is null)
            {
                _userRepository = new UserRepository(_healthUShopDbContext);
            }
            return _userRepository;
        }
    }

    public int SaveChanges()
    {
        return _healthUShopDbContext.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _healthUShopDbContext.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _healthUShopDbContext.Database.BeginTransactionAsync();
    }
}