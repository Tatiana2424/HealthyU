using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyU.DAL.Entities;

public class Recipe
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    public string? VideoUrl { get; set; }

    public int? ImageId { get; set; }
    public virtual Image? Image { get; set; }

    public bool IsPublished { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }

    public virtual RecipeNutrition? RecipeNutrition { get; set; }

    public virtual RecipeUserRating? RecipeUserRating { get; set; }

    public virtual ICollection<RecipeIngredient> Ingredients { get; set; } = new List<RecipeIngredient>();

    public virtual ICollection<RecipeInstruction> Instructions { get; set; } = new List<RecipeInstruction>();

    public virtual RecipeTimeInfo? TimeInfo { get; set; }

    public virtual ICollection<RecipeSearchKeyword> RecipeSearchKeywords { get; set; } = new List<RecipeSearchKeyword>();
}
