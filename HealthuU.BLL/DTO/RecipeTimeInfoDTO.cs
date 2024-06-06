using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.DTO;

public class RecipeTimeInfoDTO
{
    public int Id { get; set; }
    public string PrepTime { get; set; }
    public string CookTime { get; set; }
    public string CoolTime { get; set; }
    public string RestTime { get; set; }
    public string TotalTime { get; set; }
    public int Servings { get; set; }
}
