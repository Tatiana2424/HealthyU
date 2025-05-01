

using HealthuU.BLL.DTO;
using HealthuU.BLL.Services.Interfaces;

using HealthyU.Controllers.BaseController;
using HealthyU.WebApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthyU.WebApi.Controllers;

public class RecipeController : BaseApiController
{
    private readonly IRecipeService _recipeService;
    private readonly IRecipeImportService _recipeImportService;

    public RecipeController(IRecipeService recipeService, IRecipeImportService recipeImportService)
    {
        _recipeService = recipeService;
        _recipeImportService = recipeImportService;
    }

    [HttpPost]
    public async Task<IActionResult> ImportRecipes()
    {
        try
        {
            await _recipeImportService.ImportRecipesAsync();
            return Ok("Imported");
        }
        catch(Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpGet]
    [LogExecution("Getting all recipes")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _recipeService.GetAll();
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBaseRecipeData()
    {
        var result = await _recipeService.GetAllBaseRecipeData();
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBasePublishedRecipeData()
    {
        var result = await _recipeService.GetAllBasePublishedRecipeData();
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _recipeService.GetById(id);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByUserId(int id)
    {
        var result = await _recipeService.GetByUserId(id);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }

    [HttpGet]
    public async Task<IActionResult> GetUnpublishedRecipes()
    {
        var result = await _recipeService.GetUnpublishedRecipes();
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }

    [HttpGet]
    public async Task<IActionResult> GetBySearchKeyword(string searchKeyword)
    {
        var result = await _recipeService.GetBySearchKeyword(searchKeyword);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RecipeDTO recipeDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _recipeService.CreateRecipe(recipeDTO);
        if (result.IsSuccess)
        {
            return Ok(result.Value);//CreatedAtAction(nameof(GetById), new { id = result.Value });
        }
        return BadRequest(result.Error);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Publish(int id, [FromQuery] bool isPublish)
    {
        var result = await _recipeService.PublishRecipe(id, isPublish);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RecipeDTO recipeDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _recipeService.UpdateRecipe(id, recipeDTO);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }

    [HttpDelete("{recipeId}")]
    public async Task<IActionResult> Delete(int recipeId)
    {
        var result = await _recipeService.DeleteRecipeAsync(recipeId);
        if (result.IsSuccess)
        {
            return Ok();
        }
        return BadRequest(result.Error);
    }
}
