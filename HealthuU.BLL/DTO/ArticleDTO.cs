using BarberShop.BLL.DTO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.DTO;

public class ArticleDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ArticleText { get; set; } = string.Empty;
    public int ImageId { get; set; }
    public ImageDTO? Image { get; set; }
    public bool IsPublished { get; set; }
    public int? UserId { get; set; }
}

