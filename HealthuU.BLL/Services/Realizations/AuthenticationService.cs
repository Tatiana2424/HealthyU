using HealthuU.BLL.DTO;
using HealthuU.BLL.Model;
using HealthuU.BLL.Services.Interfaces;

using HealthyU.DAL.Entities;
using HealthyU.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.Services.Realizations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthenticationService(
            UserManager<User> userManager, 
            JwtSettings jwtSettings, 
            IRefreshTokenRepository refreshTokenRepository)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<(string Token, string RefreshToken, int UserId)> RegisterUserAsync(RegisterDTO registerDto)
        {
            var user = new User
            {
                UserName = registerDto.UserName,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new Exception($"User could not be created: {errors}");
            }
            await _userManager.AddToRoleAsync(user, "user");
            var jwtToken = await GenerateJwtTokenAsync(user);

            var refreshEntity = GenerateRefreshToken(user.Id);
            await _refreshTokenRepository.AddAsync(refreshEntity);

            return (jwtToken, refreshEntity.Token, user.Id);
        }

        public async Task<(string Token, string RefreshToken, int UserId)> LoginUserAsync(LoginDTO loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var jwtToken = await GenerateJwtTokenAsync(user);
                var refreshEntity = GenerateRefreshToken(user.Id);

                await _refreshTokenRepository.AddAsync(refreshEntity);

                return (jwtToken, refreshEntity.Token, user.Id);
            }

            throw new Exception("Login failed");
        }

        private async Task<string> GenerateJwtTokenAsync(User user)
        {
            var keyBytes = Encoding.ASCII.GetBytes(_jwtSettings.Key);

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,   user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(keyBytes),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }


        private RefreshToken GenerateRefreshToken(int userId)
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(7),
                UserId = userId
            };
        }

        public async Task<(string Token, string RefreshToken)> RefreshTokenAsync(string refreshToken)
        {
            var stored = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            if (stored == null || stored.Expires <= DateTime.UtcNow || stored.IsRevoked)
                throw new SecurityTokenException("Invalid refresh token");
            stored.IsRevoked = true;
            await _refreshTokenRepository.UpdateAsync(stored);

            var jwt = await GenerateJwtTokenAsync(stored.User);
            var rt = GenerateRefreshToken(stored.UserId);
            await _refreshTokenRepository.AddAsync(rt);

            return (jwt, rt.Token);
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            var stored = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            if (stored == null) return;
            stored.IsRevoked = true;
            await _refreshTokenRepository.UpdateAsync(stored);
        }

        public async Task<(string Token, string RefreshToken, int UserId)> ExternalLoginAsync(string provider, string providerKey, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new User
                {
                    UserName = email,
                    Email = email,
                    Role = "user"
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
                    throw new Exception($"External user creation failed: {errors}");
                }

                await _userManager.AddToRoleAsync(user, "user");
            }

            var jwtToken = await GenerateJwtTokenAsync(user);

            var refreshEntity = GenerateRefreshToken(user.Id);
            await _refreshTokenRepository.AddAsync(refreshEntity);

            return (jwtToken, refreshEntity.Token, user.Id);
        }
    }

}
