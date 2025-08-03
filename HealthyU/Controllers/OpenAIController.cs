using HealthuU.BLL.Services.Interfaces;

using HealthyU.Controllers.BaseController;

using Microsoft.AspNetCore.Mvc;

namespace HealthyU.WebApi.Controllers;

public class OpenAIController: BaseApiController
{
    private readonly IOpenAIService _openAIService;
    public OpenAIController(IOpenAIService openAIService) 
    {
        _openAIService = openAIService;
    }

    [HttpPost]
    public async Task<IActionResult> GetAnswer([FromBody] ChatRequestModel request)
    {
        var result = await _openAIService.GetAnswer(request.Text);
        return Ok(result);
    }

    public class ChatRequestModel
    {
        public string Text { get; set; }
    }
}
