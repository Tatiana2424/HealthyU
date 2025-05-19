using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthyU.WebApi.Controllers
{
    public class OAuthController : ControllerBase
    {
        private readonly HealthuU.BLL.Services.Interfaces.IAuthenticationService _authenticationService;

        public OAuthController(HealthuU.BLL.Services.Interfaces.IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpGet("google-signin")]
        public IActionResult GoogleSignIn()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleCallback))
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return BadRequest("Google authentication failed.");

            var providerKey = result.Principal
                                .FindFirst(ClaimTypes.NameIdentifier)
                                ?.Value;
            var email = result.Principal
                                    .FindFirst(ClaimTypes.Email)
                                    ?.Value;

            var (token, refreshToken, userId) =
                await _authenticationService.ExternalLoginAsync(
                    GoogleDefaults.AuthenticationScheme,
                    providerKey!,
                    email!);

            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok(new
            {
                token,
                refreshToken,
                userId
            });
        }

    }
}
