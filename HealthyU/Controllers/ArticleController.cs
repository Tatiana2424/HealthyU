using HealthuU.BLL.DTO;
using HealthuU.BLL.Services.Interfaces;
using HealthuU.BLL.Services.Interfaces.Logging;
using HealthyU.Controllers.BaseController;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthyU.WebApi.Controllers;

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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        //_logger.LogInformation("Request to retrieve all articles");
        _logger.LogInformation("Logger1 ID");
        _logger1.LogInformation("Logger2 ID");
        var result = await _articleService.GetAllArticles();
        if (result.IsSuccess)
        {
            _logger.LogInformation($"Retrieved {result.Value.Count()} articles");
            return Ok(result.Value);
        }
        _logger.LogWarning($"Failed to retrieve articles: {result.Error}");
        return NotFound(result.Error);
    }

    [HttpGet]
    public async Task<IActionResult> GetUnpublishedArticles()
    {
        var result = await _articleService.GetUnpublishedArticles();
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }


    [HttpGet]
    public async Task<IActionResult> ImportToFile()
    {
        var result = await _articleService.ImportArticlesToJsonAsync();
        if (result.IsSuccess)
        {
            return Ok(result.IsSuccess);
        }
        return NotFound(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _articleService.GetArticleById(id);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByUsetrId(int id)
    {
        var result = await _articleService.GetArticlesByUserId(id);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetAllArticlesWithoutSelected(int id)
    {
        var result = await _articleService.GetAllArticlesWithoutSelected(id);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }

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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ArticleDTO articleDTO)
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

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ArticleDTO articleDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _articleService.UpdateArticle(id, articleDTO);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Publish(int id, [FromQuery] bool isPublish)
    {
        var result = await _articleService.PublishArticle(id, isPublish);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }

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
