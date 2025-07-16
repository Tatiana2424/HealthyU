using HealthuU.BLL.DTO;
using HealthuU.BLL.Helpers;
using HealthuU.BLL.Model;
using HealthuU.BLL.Services.Interfaces.Logging;
using HealthuU.BLL.Services.Interfaces;
using HealthuU.BLL.Services.Realizations;
using HealthyU.DAL.Repositories.Interfaces;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HealthyU.Tests;

public class FakeHttpHandler : HttpMessageHandler
{
    private readonly string _json;
    public FakeHttpHandler(string json) => _json = json;
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var resp = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(_json, Encoding.UTF8, "application/json")
        };
        return Task.FromResult(resp);
    }
}

public class RecipeImportServiceTests
{
    private ApiRecipe CreateFullDummyRecipe(string name)
    {
        return new ApiRecipe
        {
            Name = name,
            Description = "",
            OriginalVideoUrl = "",
            ThumbnailUrl = "",
            Nutrition = new Nutrition
            {
                Calories = 0,
                Carbohydrates = 0,
                Fat = 0,
                Fiber = 0,
                Protein = 0
            },
            Sections =
                [
                    new Section { Components = [] }
                ],
            Instructions = [],
            NumServings = 1,
            UserRatings = new UserRatings
            {
                CountPositive = 0,
                CountNegative = 0,
                Score = 0.0
            },
            Keywords = "",
            TotalTimeMinutes = 0,
            TotalTimeTier = new TotalTimeTier { DisplayTier = null }
        };
    }


    [Fact]
    public async Task ImportRecipesAsync_CreatesOnlyNewRecipes()
    {
        // Arrange
        var apiResponse = new ApiResponse
        {
            Results =
            [
                CreateFullDummyRecipe("Exist"),
                CreateFullDummyRecipe("NewOne")
            ]
        };

        var rawJson = JsonConvert.SerializeObject(apiResponse);

        var handler = new FakeHttpHandler(rawJson);
        var httpClient = new HttpClient(handler);

        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory
            .Setup(f => f.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);

        var existing = new List<RecipeDTO>
        {
            new() { Name = "Exist" }
        };
        var mockRecipeService = new Mock<IRecipeService>();
        mockRecipeService
            .Setup(s => s.GetAllBaseRecipeData())
            .ReturnsAsync(Result.Success(existing));

        var created = new List<RecipeDTO>();
        mockRecipeService
            .Setup(s => s.CreateRecipeAsync(It.IsAny<RecipeDTO>()))
            .Callback<RecipeDTO>(created.Add)
            .Returns(Task.CompletedTask);

        var svc = new RecipeImportService(
            mockRecipeService.Object,
            mockFactory.Object,
            Mock.Of<IRecipeRepository>(),          
            Mock.Of<ILoggerService<FileResourceHolder>>()    
        );

        // Act
        await svc.ImportRecipesAsync();

        // Assert:
        Assert.Single(created);         
        Assert.Equal("NewOne", created[0].Name);    
    }



    [Fact]
    public async Task ImportRecipesAsync_WhenNoResults_DoesNotCallCreate()
    {
        // Arrange
        var rawJson = JsonConvert.SerializeObject(new ApiResponse { Results = null });
        var handler = new FakeHttpHandler(rawJson);
        var httpClient = new HttpClient(handler);
        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory
            .Setup(f => f.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);

        var mockRecipeService = new Mock<IRecipeService>();
        mockRecipeService
            .Setup(s => s.GetAllBaseRecipeData())
            .ReturnsAsync(Result.Success(new List<RecipeDTO>()));

        int createCalls = 0;
        mockRecipeService
            .Setup(s => s.CreateRecipeAsync(It.IsAny<RecipeDTO>()))
            .Callback(() => createCalls++)
            .Returns(Task.CompletedTask);

        var svc = new RecipeImportService(
            mockRecipeService.Object,
            mockFactory.Object,
            Mock.Of<IRecipeRepository>(),
            Mock.Of<ILoggerService<FileResourceHolder>>());

        // Act
        await svc.ImportRecipesAsync();

        // Assert:
        Assert.Equal(0, createCalls);
    }
}
