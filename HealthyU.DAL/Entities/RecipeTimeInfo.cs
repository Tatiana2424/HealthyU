using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyU.DAL.Entities;

public class RecipeTimeInfo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string PrepTime { get; set; }

    [Required]
    public string CookTime { get; set; }

    [Required]
    public string CoolTime { get; set; }

    [Required]
    public string RestTime { get; set; }

    [Required]
    public string TotalTime { get; set; }
    
    [Required]
    public int Servings { get; set; }

    [ForeignKey("Recipe")]
    public int RecipeId { get; set; }
    public virtual Recipe Recipe { get; set; }
}
