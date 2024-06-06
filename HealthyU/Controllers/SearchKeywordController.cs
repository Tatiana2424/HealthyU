using HealthuU.BLL.Services.Interfaces;

using HealthyU.Controllers.BaseController;
using HealthyU.DAL.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace HealthyU.WebApi.Controllers;

public class SearchKeywordController : BaseApiController
{
    private readonly ISearchKeywordService _searchKeywordService;

    public SearchKeywordController(ISearchKeywordService searchKeywordService)
    {
        _searchKeywordService = searchKeywordService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRandom()
    {
        return Ok();
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _searchKeywordService.GetAllSearchKeywords();
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }
}
