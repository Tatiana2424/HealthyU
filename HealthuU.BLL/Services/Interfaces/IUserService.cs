using CSharpFunctionalExtensions;

using HealthuU.BLL.DTO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.Services.Interfaces;

public interface IUserService
{
    Task<Result<UserDTO>> GetUserById(int userId);
    Task<Result<UserDTO>> UpdateUser(int userId, UserDTO userDTO);
    Task<int> CountUsersAsync();
}
