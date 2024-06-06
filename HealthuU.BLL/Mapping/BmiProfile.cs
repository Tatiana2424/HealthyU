using AutoMapper;
using BarberShop.BLL.DTO;

using HealthuU.BLL.DTO;

using HealthyU.DAL.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.Mapping;

public class BmiProfile : Profile
{
    public BmiProfile()
    {
        CreateMap<BMIData, BmiDTO>()
            .ForMember(
                dest => dest.DateTime,
                opt => opt.MapFrom(src => src.DateTime.ToString("yyyy-MM-dd")) 
            )
            .ReverseMap();
    }
}
