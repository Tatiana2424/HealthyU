using Asp.Versioning;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace HealthyU.Controllers.BaseController;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[Route("api/[controller]/[action]")]
public class BaseApiController : ControllerBase
{
    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return result.Value is null ?
                NotFound("Found result matching null") : Ok(result.Value);
        }

        return BadRequest(result.Error);
    }
}