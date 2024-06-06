using AutoMapper;
using BarberShop.BLL.DTO;

using HealthuU.BLL.DTO;

using HealthyU.DAL.Entities;

namespace HealthuU.BLL.Mapping;

public class ArticleProfile : Profile
{
    public ArticleProfile()
    {
        CreateMap<Article, ArticleDTO>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image != null ? src.Image : null))
            .ReverseMap();
    }
}

