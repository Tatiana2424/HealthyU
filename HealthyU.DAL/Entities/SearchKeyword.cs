using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyU.DAL.Entities;

public class SearchKeyword
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Keyword { get; set; }

    public virtual ICollection<RecipeSearchKeyword> RecipeSearchKeywords { get; set; } = new List<RecipeSearchKeyword>();
}
