using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BarberShop.BLL.DTO;

public class BarberDTO
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public string? Position { get; set; }

    public int? Experience { get; set; }

    public int ImageId { get; set; }

    public double Rate { get; set; }
}
