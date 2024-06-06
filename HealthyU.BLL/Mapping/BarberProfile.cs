using AutoMapper;
using BarberShop.BLL.DTO;
using BarberShop.DAL.Entities;

namespace BarberShop.BLL.Mapping;

public class BarberProfile: Profile
{
    public BarberProfile()
    {
        CreateMap<Barber, BarberDTO>().ReverseMap();
    }
}

