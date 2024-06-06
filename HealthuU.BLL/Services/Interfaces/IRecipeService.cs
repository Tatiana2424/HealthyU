using CSharpFunctionalExtensions;

using HealthuU.BLL.DTO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.Services.Interfaces;

public interface IRecipeService
{
    Task CreateRecipeAsync(RecipeDTO recipeDto);
    Task<Result<List<RecipeDTO>>> GetAllBaseRecipeData();
    Task<Result<List<RecipeDTO>>> GetAll();
    Task<Result<RecipeDTO>> GetById(int recipeId);
    Task<Result<List<RecipeDTO>>> GetBySearchKeyword(string searchKeyword);
    Task<Result<int>> CreateRecipe(RecipeDTO recipeDto);
    Task<Result<RecipeDTO>> UpdateRecipe(int recipeId, RecipeDTO recipeDto);
    Task<Result<bool>> DeleteRecipeAsync(int recipeId);
    Task<Result<RecipeDTO>> PublishRecipe(int recipeId, bool isPublish);
    Task<Result<List<RecipeDTO>>> GetAllBasePublishedRecipeData();
    Task<Result<List<RecipeDTO>>> GetByUserId(int userId);
    Task<Result<List<RecipeDTO>>> GetUnpublishedRecipes();
}
