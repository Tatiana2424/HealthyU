using HealthuU.BLL.DTO;
using HealthuU.BLL.Services.Interfaces;

using HealthyU.Controllers.BaseController;

using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var (token, userId) = await _authenticationService.RegisterUserAsync(registerDto);
                return Ok(new { token, userId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var (token, userId) = await _authenticationService.LoginUserAsync(loginDto);
                return Ok(new { token, userId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
