using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.DTO;

public class RecipeNutritionDTO
{
    public int Id { get; set; }
    public int Protein { get; set; }
    public int Fat { get; set; }
    public int Calories { get; set; }
    public int Carbohydrates { get; set; }
    public int Fiber { get; set; }
}
