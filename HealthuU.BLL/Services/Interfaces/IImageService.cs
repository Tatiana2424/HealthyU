using BarberShop.BLL.DTO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.Services.Interfaces
{
    public interface IImageService
    {
        Task<ImageDTO> CreateOrUpdateImageAsync(ImageDTO imageDTO);
    }
}
