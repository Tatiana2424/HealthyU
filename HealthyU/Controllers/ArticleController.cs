using Asp.Versioning;
using HealthuU.BLL.DTO;
using HealthuU.BLL.Services.Interfaces;
using HealthuU.BLL.Services.Interfaces.Logging;
using HealthyU.Controllers.BaseController;
using HealthyU.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthyU.WebApi.Controllers;

[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class ArticleController : BaseApiController
{
    public readonly IArticleService _articleService;
    private readonly ILoggerService<ArticleController> _logger;
    private readonly ILoggerService<ArticleController> _logger1;

    public ArticleController(IArticleService articleService, 
        ILoggerService<ArticleController> logger,
        ILoggerService<ArticleController> logger1)
    {
        _articleService = articleService;
        _logger = logger;
        _logger1 = logger1;
    }
    /// <summary>
    /// Retrieves all articles using the original v1 implementation.
    /// </summary>
    [HttpGet]
    [MapToApiVersion("1.0")]
    [ActionName("GetAll")]
    public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetAllV1()
    {
        _logger.LogInformation("Logger1 ID");
        _logger1.LogInformation("Logger2 ID");
        var result = await _articleService.GetAllArticles();
        if (result.IsSuccess)
        {
            _logger.LogInformation($"Retrieved {result.Value.Count()} articles");
            //throw new NullReferenceException("Test exception for middleware");
            return Ok(result.Value);
        }
        _logger.LogWarning($"Failed to retrieve articles: {result.Error}");
        return NotFound(result.Error);
    }

    /// <summary>
    /// Retrieves all articles using the original v2 implementation.
    /// </summary>
    [HttpGet]
    [ActionName("GetAll")]
    [MapToApiVersion("2.0")]
    public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetAllV2()
    {
        var result = await _articleService.GetAllArticles();
        return HandleResult(result);
    }

    [MapToApiVersion("1.0")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetUnpublishedArticles()
    {
        var result = await _articleService.GetUnpublishedArticles();
        return HandleResult(result);
    }

    [MapToApiVersion("1.0")]
    [HttpGet]
    public async Task<ActionResult> ImportToFile()
    {
        var result = await _articleService.ImportArticlesToJsonAsync();
        if (result.IsSuccess)
        {
            return Ok();
        }
        return BadRequest(result.Error);
    }

    [MapToApiVersion("1.0")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ArticleDTO>> GetById(int id)
    {
        var result = await _articleService.GetArticleById(id);
        return HandleResult(result);
    }

    [MapToApiVersion("1.0")]
    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetByUsetrId(int id)
    {
        var result = await _articleService.GetArticlesByUserId(id);
        return HandleResult(result);
    }

    [MapToApiVersion("1.0")]
    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetAllArticlesWithoutSelected(int id)
    {
        var result = await _articleService.GetAllArticlesWithoutSelected(id);
        return HandleResult(result);
    }

    [MapToApiVersion("1.0")]
    [HttpPost]
    public async Task<IActionResult> ImportFromFile()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _articleService.ImportArticlesFromJsonFileAsync();
        if (result.IsSuccess)
        {
            return Ok(result.IsSuccess);
        }
        return BadRequest(result.Error);
    }

    [MapToApiVersion("1.0")]
    [HttpPost]
    public async Task<ActionResult<ArticleDTO>> Create([FromBody] ArticleDTO articleDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _articleService.CreateArticle(articleDTO);
        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }
        return BadRequest(result.Error);
    }

    [MapToApiVersion("1.0")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ArticleDTO>> Update(int id, [FromBody] ArticleDTO articleDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _articleService.UpdateArticle(id, articleDTO);
        return HandleResult(result);
    }

    [MapToApiVersion("1.0")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ArticleDTO>> Publish(int id, [FromQuery] bool isPublish)
    {
        var result = await _articleService.PublishArticle(id, isPublish);
        return HandleResult(result);
    }

    [MapToApiVersion("1.0")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _articleService.DeleteArticle(id);
        if (result.IsSuccess)
        {
            return NoContent();
        }
        return BadRequest(result.Error);
    }
}
