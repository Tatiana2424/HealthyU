using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.DTO
{
    public class BmiDTO
    {
        public int Id { get; set; }

        public double Height { get; set; }

        public double Weight { get; set; }

        public double BMI { get; set; }

        public string Classification { get; set; }

        public int UserId { get; set; }
        public string? DateTime { get; set; }
    }
}
