using AutoMapper;

using CSharpFunctionalExtensions;

using HealthuU.BLL.DTO;
using HealthuU.BLL.Services.Realizations;

using HealthyU.DAL.Entities;
using HealthyU.DAL.Repositories.Interfaces.Base;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthuU.BLL.Services.Interfaces;

public class ArticleService : IArticleService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IImageService _imageService;
    public ArticleService(IRepositoryWrapper repositoryWrapper, IMapper mapper, IImageService imageService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _imageService = imageService;
    }

    public async Task<Result<List<ArticleDTO>>> GetAllArticles()
    {
        var articles = await _repositoryWrapper.ArticleRepository.GetAllAsync(include: a => a.Where(x => x.IsPublished == true).Include(x => x.Image));
        var articlesDTO = _mapper.Map<List<ArticleDTO>>(articles);
        return Result.Success(articlesDTO);
    }

    public async Task<Result<List<ArticleDTO>>> GetAllArticlesWithoutSelected(int id)
    {
        var articles = await _repositoryWrapper.ArticleRepository.GetAllAsync(include: a => a.Where(x => x.Id != id).Include(x => x.Image));
        var articlesDTO = _mapper.Map<List<ArticleDTO>>(articles);
        return Result.Success(articlesDTO);
    }

    public async Task<Result<List<ArticleDTO>>> GetUnpublishedArticles()
    {
        var articles = await _repositoryWrapper.ArticleRepository.GetAllAsync(include: a => a.Where(x => x.IsPublished == false).Include(x => x.Image));
        var articlesDTO = _mapper.Map<List<ArticleDTO>>(articles);
        return Result.Success(articlesDTO);
    }

    public async Task<Result<ArticleDTO>> GetArticleById(int articleId)
    {
        var articles = await _repositoryWrapper.ArticleRepository.GetFirstOrDefaultAsync(a => a.Id == articleId, include: a => a.Include(x => x.Image));
        var articlesDTO = _mapper.Map<ArticleDTO>(articles);
        return Result.Success(articlesDTO);
    }
    public async Task<Result<List<ArticleDTO>>> GetArticlesByUserId(int userId)
    {
        var articles = await _repositoryWrapper.ArticleRepository.GetAllAsync(a => a.UserId == userId, include: a => a.Include(x => x.Image));
        var articlesDTO = _mapper.Map<List<ArticleDTO>>(articles);
        return Result.Success(articlesDTO);
    }

    public async Task<Result> ImportArticlesToJsonAsync()
    {
        var articles = await _repositoryWrapper.ArticleRepository.GetAllAsync(include: a => a.Include(x => x.Image));
        var articlesDTO = _mapper.Map<List<ArticleDTO>>(articles);

        // Серіалізація в JSON
        string jsonString = System.Text.Json.JsonSerializer.Serialize(articlesDTO, new JsonSerializerOptions { WriteIndented = true });

        // Шлях для збереження файлу на робочому столі
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(desktopPath, "articles.json");

        // Запис JSON у файл
        await File.WriteAllTextAsync(filePath, jsonString);
        return Result.Success();
    }


    public async Task<Result<ArticleDTO>> CreateArticle(ArticleDTO articleDTO)
    {
        if (articleDTO.Image != null)
        {
            var imageDTO = await _imageService.CreateOrUpdateImageAsync(articleDTO.Image);
            articleDTO.ImageId = imageDTO.Id;
        }

        var article = _mapper.Map<Article>(articleDTO);
        _repositoryWrapper.ArticleRepository.Create(article);
        await _repositoryWrapper.SaveChangesAsync();
        return Result.Success(_mapper.Map<ArticleDTO>(article));
    }


    public async Task<Result> ImportArticlesFromJsonFileAsync()
    {
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(desktopPath, "articles.json");
        string jsonString = await File.ReadAllTextAsync(filePath);
        var articlesDTO = System.Text.Json.JsonSerializer.Deserialize<List<ArticleDTO>>(jsonString);

        if (articlesDTO == null)
        {
            return Result.Failure("Failed to deserialize JSON.");
        }

        foreach (var articleDTO in articlesDTO)
        {
            if (articleDTO.Image != null)
            {
                var imageDTO = await _imageService.CreateOrUpdateImageAsync(articleDTO.Image);
                articleDTO.ImageId = imageDTO.Id;
            }

            var article = _mapper.Map<Article>(articleDTO);
            _repositoryWrapper.ArticleRepository.Create(article);
        }

        await _repositoryWrapper.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<bool>> DeleteArticle(int articleId)
    {
        var article = await _repositoryWrapper.ArticleRepository.GetFirstOrDefaultAsync(a => a.Id == articleId);
        if (article == null)
        {
            return Result.Failure<bool>("Article not found.");
        }

        _repositoryWrapper.ArticleRepository.Delete(article);
        await _repositoryWrapper.SaveChangesAsync();
        return Result.Success(true);
    }

    public async Task<Result<ArticleDTO>> UpdateArticle(int articleId, ArticleDTO articleDTO)
    {
        var article = await _repositoryWrapper.ArticleRepository.GetFirstOrDefaultAsync(a => a.Id == articleId);
        if (article == null)
        {
            return Result.Failure<ArticleDTO>("Article not found.");
        }

        if (articleDTO.Image != null)
        {
            var imageDTO = await _imageService.CreateOrUpdateImageAsync(articleDTO.Image);
            articleDTO.ImageId = imageDTO.Id; 
        }
        else if (articleDTO.ImageId == 0)
        {
            article.ImageId = 0;
            article.Image = null;
        }

        _mapper.Map(articleDTO, article);
        _repositoryWrapper.ArticleRepository.Update(article);
        await _repositoryWrapper.SaveChangesAsync();
        return Result.Success(_mapper.Map<ArticleDTO>(article));
    }

    public async Task<Result<ArticleDTO>> PublishArticle(int id, bool isPublish)
    {
        var article = await _repositoryWrapper.ArticleRepository.GetFirstOrDefaultAsync(a => a.Id == id && a.IsPublished != isPublish);
        if (article == null)
        {
            return Result.Failure<ArticleDTO>("Article not found.");
        }

        article.IsPublished = isPublish;
        _repositoryWrapper.ArticleRepository.Update(article);
        await _repositoryWrapper.SaveChangesAsync();
        return Result.Success(_mapper.Map<ArticleDTO>(article));
    }
}
