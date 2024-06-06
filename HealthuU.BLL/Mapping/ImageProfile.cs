using AutoMapper;
using BarberShop.BLL.DTO;
using HealthyU.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberShop.BLL.Mapping;

public class ImageProfile: Profile
{
    public ImageProfile()
    {
        CreateMap<Image, ImageDTO>().ReverseMap();
    }
}
