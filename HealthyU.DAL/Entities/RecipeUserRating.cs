using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyU.DAL.Entities;

public class RecipeUserRating
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey("Recipe")]
    public int RecipeId { get; set; }
    public virtual Recipe Recipe { get; set; }

    public int CountPositive { get; set; }

    public int CountNegative { get; set; }

    public int Score { get; set; }
}
