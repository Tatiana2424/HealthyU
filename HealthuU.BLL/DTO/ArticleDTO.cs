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
    public string Title { get; set; }
    public string Description { get; set; }
    public string ArticleText { get; set; }
    public int ImageId { get; set; }
    public ImageDTO? Image { get; set; }
    public bool IsPublished { get; set; }
    public int? UserId { get; set; }
}

