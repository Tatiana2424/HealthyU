using BarberShop.BLL.DTO;
using HealthuU.BLL.DTO;
using HealthuU.BLL.Helpers;
using HealthuU.BLL.Model;
using HealthuU.BLL.Services.Interfaces;
using HealthuU.BLL.Services.Interfaces.Logging;
using HealthyU.DAL.Repositories.Interfaces;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace HealthuU.BLL.Services.Realizations;

public class RecipeImportService: IRecipeImportService
{
    private readonly IRecipeService _recipeService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IRecipeRepository _recipeRepository;
    private readonly ILoggerService<FileResourceHolder> _logger;


    public RecipeImportService(
        IRecipeService recipeService, 
        IHttpClientFactory httpClientFactory, 
        IRecipeRepository recipeRepository,
        ILoggerService<FileResourceHolder> logger)
    {
        _recipeService = recipeService;
        _httpClientFactory = httpClientFactory;
        _recipeRepository = recipeRepository;
        _logger = logger;
    }

    public async Task ImportRecipesAsync()
    {
        var client = _httpClientFactory.CreateClient();
        var requestUri = new Uri("https://tasty.p.rapidapi.com/recipes/list?from=0&size=200&tags=healthy");
        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        request.Headers.Add("X-RapidAPI-Key", "106bda5ae5mshe586f4d4be8f584p1280b9jsnc924f17b101e");
        request.Headers.Add("X-RapidAPI-Host", "tasty.p.rapidapi.com");

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);

        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var filePath = Path.Combine(desktopPath, "recipes_healthy.json");
        await File.WriteAllTextAsync(filePath, content);

        if (apiResponse?.Results == null)
        {
            return;
        }

        var existingRecipesResult = await _recipeService.GetAllBaseRecipeData();
        if (!existingRecipesResult.IsSuccess)
        {
            return;
        }
        var existingRecipes = existingRecipesResult.Value.Select(r => r.Name).ToList();

        foreach (var apiRecipe in apiResponse.Results)
        {
            if (!existingRecipes.Contains(apiRecipe.Name))
            {
                var recipeDTO = MapApiRecipeToDTO(apiRecipe);
                await _recipeService.CreateRecipeAsync(recipeDTO);
            }
        }
    }

    public async Task ImportRecipesFromDesktopAsync()
    {
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var filePath = Path.Combine(desktopPath, "recipes_healthy.json");

        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found.");
            return;
        }

        var content = await File.ReadAllTextAsync(filePath);

        var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);

        if (apiResponse?.Results is null)
        {
            return;
        }

        var existingRecipesResult = await _recipeService.GetAllBaseRecipeData();
        if (!existingRecipesResult.IsSuccess)
        {
            return;
        }
        var existingRecipes = existingRecipesResult.Value.Select(r => r.Name).ToList();

        foreach (var apiRecipe in apiResponse.Results)
        {
            if (!existingRecipes.Contains(apiRecipe.Name))
            {
                var recipeDTO = MapApiRecipeToDTO(apiRecipe);
                await _recipeService.CreateRecipeAsync(recipeDTO);
            }
        }
    }

    public async Task ImportRecipesFromDesktopWithIDisposableAsync()
    {
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var filePath = Path.Combine(desktopPath, "recipes_healthy.json");

        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found.");
            return;
        }

        using var holder = new FileResourceHolder(filePath, _logger);
        var content = await holder.ReadAllTextAsync();

        var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);

        if (apiResponse?.Results is null)
            return;

        var existingRecipesResult = await _recipeService.GetAllBaseRecipeData();
        if (!existingRecipesResult.IsSuccess)
            return;

        var existingRecipes = existingRecipesResult.Value.Select(r => r.Name).ToList();

        foreach (var apiRecipe in apiResponse.Results)
        {
            if (!existingRecipes.Contains(apiRecipe.Name))
            {
                var recipeDTO = MapApiRecipeToDTO(apiRecipe);
                await _recipeService.CreateRecipeAsync(recipeDTO);
            }
        }
    }

    private RecipeDTO MapApiRecipeToDTO(ApiRecipe apiRecipe)
    {
        var recipeDTO = new RecipeDTO
        {
            Name = apiRecipe.Name,
            Description = apiRecipe.Description,
            VideoUrl = apiRecipe.OriginalVideoUrl,
            Image = new ImageDTO { Url = apiRecipe.ThumbnailUrl },
            IsPublished = true,
            User = null,
            Ingredients = apiRecipe.Sections.SelectMany(section => section.Components.Select(component => new RecipeIngredientDTO
            {
                Name = component.RawText,
                Position = component.Position
            })).ToList(),
            Instructions = apiRecipe.Instructions.Select(instruction => new RecipeInstructionDTO
            {
                DisplayText = instruction.DisplayText,
                Position = instruction.Position
            }).ToList(),
            RecipeNutrition = new RecipeNutritionDTO
            {
                Calories = apiRecipe.Nutrition.Calories,
                Carbohydrates = apiRecipe.Nutrition.Carbohydrates,
                Fat = apiRecipe.Nutrition.Fat,
                Fiber = apiRecipe.Nutrition.Fiber,
                Protein = apiRecipe.Nutrition.Protein
            },
            RecipeUserRating = new RecipeUserRatingDTO
            {
                CountPositive = apiRecipe.UserRatings.CountPositive,
                CountNegative = apiRecipe.UserRatings.CountNegative,
                Score = ConvertScore(apiRecipe.UserRatings.Score)
            },
            SearchKeywords = apiRecipe.Keywords?.Split(", ").Select(k => new SearchKeywordDTO { Keyword = k.Trim() }).ToList()
        };

        SetRecipeTimeInfo(recipeDTO, apiRecipe.TotalTimeMinutes, apiRecipe?.TotalTimeTier?.DisplayTier, apiRecipe.NumServings);

        return recipeDTO;
    }

    private int ConvertScore(double apiScore)
    {
        return (int)Math.Round(apiScore * 5);
    }

    private void SetRecipeTimeInfo(RecipeDTO recipeDTO, int? totalTimeMinutes, string displayTier, int numServings)
    {
        int cookTime = totalTimeMinutes ?? ConvertTimeTierToMinutes(displayTier);
        int prepTime = cookTime / 2;
        int coolTime = 5;
        int restTime = 10;
        int totalTime = cookTime + prepTime + coolTime + restTime;

        recipeDTO.TimeInfo = new RecipeTimeInfoDTO
        {
            CookTime = cookTime.ToString(),
            PrepTime = prepTime.ToString(),
            CoolTime = coolTime.ToString(),
            RestTime = restTime.ToString(),
            TotalTime = totalTime.ToString(),
            Servings = numServings
        };
    }

    private int ConvertTimeTierToMinutes(string displayTier)
    {
        if(displayTier is null)
        {
            return 0;
        }

        var match = Regex.Match(displayTier, @"\d+");
        if (match.Success)
        {
            return int.Parse(match.Value);
        }
        return 0;
    }
}