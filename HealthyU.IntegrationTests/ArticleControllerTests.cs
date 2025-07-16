using BarberShop.BLL.DTO;
using FluentAssertions;
using HealthuU.BLL.DTO;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace HealthyU.IntegrationTests;

public class ArticleControllerTests : IClassFixture<CustomWebAppFactory<HealthyU.WebApi.Program>>
{
    private readonly HttpClient _client;

    public ArticleControllerTests(CustomWebAppFactory<HealthyU.WebApi.Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsPublishedArticles()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/Article/GetAll");
        response.EnsureSuccessStatusCode();

        // Assert
        var json = await response.Content.ReadAsStringAsync();
        var articles = JsonSerializer.Deserialize<List<ArticleDTO>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        articles.Should().NotBeNull();
        articles.Should().ContainSingle(a => a.Id == 1 && a.IsPublished);
    }

    [Fact]
    public async Task GetById_ExistingId_ReturnsArticleDto()
    {
        // Arrange
        const int existingId = 1;

        // Act
        var response = await _client.GetAsync($"/api/v1/Article/GetById/{existingId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await response.Content.ReadAsStringAsync();
        var article = JsonSerializer.Deserialize<ArticleDTO>(
            json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        article.Should().NotBeNull();
        article.Id.Should().Be(existingId);
        article.IsPublished.Should().BeTrue();
    }

    [Fact]
    public async Task GetById_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        const int nonExistingId = 999;

        // Act
        var response = await _client.GetAsync($"/api/v1/Article/GetById/{nonExistingId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_ValidArticle_ReturnsCreatedArticle()
    {
        // Arrange
        var newArticle = new ArticleDTO
        {
            Title = "Integration Test Title",
            Description = "Integration Test Description",
            ArticleText = "Test text",
            Image = new ImageDTO
            {
                Title = "img-title",
                Alt = "img-alt",
                Url = "/images/test.jpg"
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/Article/Create", newArticle);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await response.Content.ReadFromJsonAsync<ArticleDTO>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        created.Should().NotBeNull();
        created!.Id.Should().BeGreaterThan(0);
        created.Title.Should().Be(newArticle.Title);
        created.Description.Should().Be(newArticle.Description);
        created.ArticleText.Should().Be(newArticle.ArticleText);
        created.UserId.Should().Be(newArticle.UserId);
        created.IsPublished.Should().BeFalse();
        created.Image.Should().NotBeNull();
        created.Image!.Url.Should().Be(newArticle.Image.Url);
    }

}