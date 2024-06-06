using BarberShop.BLL.DTO;

namespace HealthuU.BLL.DTO;

public class RecipeDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? VideoUrl { get; set; }
    public int? ImageId { get; set; }
    public ImageDTO? Image { get; set; }
    public bool IsPublished { get; set; }
    public int? UserId { get; set; }
    public UserDTO? User { get; set; }
    public RecipeNutritionDTO? RecipeNutrition { get; set; }
    public RecipeUserRatingDTO? RecipeUserRating { get; set; }
    public ICollection<RecipeIngredientDTO> Ingredients { get; set; } = new List<RecipeIngredientDTO>();
    public ICollection<RecipeInstructionDTO> Instructions { get; set; } = new List<RecipeInstructionDTO>();
    public RecipeTimeInfoDTO? TimeInfo { get; set; }
    public ICollection<SearchKeywordDTO> SearchKeywords { get; set; } = new List<SearchKeywordDTO>();
}

