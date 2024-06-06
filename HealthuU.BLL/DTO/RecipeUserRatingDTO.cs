using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.DTO;

public class RecipeUserRatingDTO
{
    public int Id { get; set; }
    public int CountPositive { get; set; }
    public int CountNegative { get; set; }
    public int Score { get; set; }
}
