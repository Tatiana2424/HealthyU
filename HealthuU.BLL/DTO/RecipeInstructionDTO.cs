using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.DTO;

public class RecipeInstructionDTO
{
    public int Id { get; set; }
    public string? DisplayText { get; set; }
    public int Position { get; set; }
}
