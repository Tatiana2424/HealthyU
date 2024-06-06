using AutoMapper;
using HealthuU.BLL.DTO;
using HealthyU.DAL.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.Mapping;

public class SearchKeywordProfile : Profile
{
    public SearchKeywordProfile()
    {
        CreateMap<SearchKeyword, SearchKeywordDTO>()
            .ReverseMap();
    }
}
