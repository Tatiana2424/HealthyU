using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberShop.BLL.DTO;

public class ImageDTO
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Alt { get; set; }

    public string Url { get; set; }
}
