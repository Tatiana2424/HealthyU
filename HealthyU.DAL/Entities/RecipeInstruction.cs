using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyU.DAL.Entities;

public class RecipeInstruction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int Position { get; set; }

    public string? DisplayText { get; set; }

    [ForeignKey("Recipe")]
    public int RecipeId { get; set; }
    public virtual Recipe Recipe { get; set; }
}
