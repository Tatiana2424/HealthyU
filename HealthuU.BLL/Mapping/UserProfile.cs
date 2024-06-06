using AutoMapper;
using HealthuU.BLL.DTO;
using HealthyU.DAL.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>()
            .ReverseMap();
    }
}

