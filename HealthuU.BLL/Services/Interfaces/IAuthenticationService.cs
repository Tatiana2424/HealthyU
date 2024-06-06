using HealthuU.BLL.DTO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<(string Token, int UserId)> LoginUserAsync(LoginDto loginDto);
        Task<(string Token, int UserId)> RegisterUserAsync(RegisterDto registerDto);
    }
}
