using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyU.DAL.Entities;

public class RecipeNutrition
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey("Recipe")]
    public int RecipeId { get; set; }
    public virtual Recipe Recipe { get; set; }

    public int Protein { get; set; }

    public int Fat { get; set; }

    public int Calories { get; set; }

    public int Carbohydrates { get; set; }

    public int Fiber { get; set; }
}
