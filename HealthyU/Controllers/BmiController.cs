using HealthuU.BLL.DTO;
using HealthuU.BLL.Services.Interfaces;

using HealthyU.Controllers.BaseController;

using Microsoft.AspNetCore.Mvc;

namespace HealthyU.WebApi.Controllers;

public class BmiController : BaseApiController
{
    private readonly IBmiService _bmiService;
    public BmiController(IBmiService bmiService)
    {
        _bmiService = bmiService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByUserId(int id)
    {
        var result = await _bmiService.GetBmiByUserId(id);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BmiDTO bmiDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _bmiService.CreateBMI(bmiDTO);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _bmiService.DeleteBMI(id);
        if (result.IsSuccess)
        {
            return NoContent();
        }
        return BadRequest(result.Error);

    }
}
