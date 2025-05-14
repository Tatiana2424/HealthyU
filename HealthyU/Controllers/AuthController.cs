using HealthuU.BLL.DTO;
using HealthuU.BLL.Services.Interfaces;

using HealthyU.Controllers.BaseController;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HealthyU.WebApi.Controllers
{

    public class AuthController : BaseApiController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                var (token, refreshToken, userId) = await _authenticationService.RegisterUserAsync(registerDto);
                return Ok(new
                {
                    token,
                    refreshToken,
                    userId
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                var (token, refreshToken, userId) = await _authenticationService.LoginUserAsync(loginDto);
                return Ok(new
                {
                    token,
                    refreshToken,
                    userId
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Refresh([FromBody] RefreshDTO dto)
        {
            try
            {
                var (token, refreshToken) = await _authenticationService.RefreshTokenAsync(dto.RefreshToken);
                return Ok(new
                {
                    token,
                    refreshToken
                });
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Revoke([FromBody] RefreshDTO dto)
        {
            await _authenticationService.RevokeRefreshTokenAsync(dto.RefreshToken);
            return NoContent();
        }
    }
}
