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
        Task<(string Token, string RefreshToken, int UserId)> LoginUserAsync(LoginDTO loginDto);
        Task<(string Token, string RefreshToken, int UserId)> RegisterUserAsync(RegisterDTO registerDto);
        Task<(string Token, string RefreshToken)> RefreshTokenAsync(string token);
        Task RevokeRefreshTokenAsync(string token);
        Task<(string Token, string RefreshToken, int UserId)> ExternalLoginAsync(
            string provider,
            string providerKey,
            string email
        );
    }
}
