using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyU.DAL.Entities;

public class RecipeSearchKeyword
{
    public int RecipeId { get; set; }
    public virtual Recipe Recipe { get; set; }

    public int KeywordId { get; set; }
    public virtual SearchKeyword SearchKeyword { get; set; }
}
