using AutoMapper;

using CSharpFunctionalExtensions;

using HealthuU.BLL.DTO;
using HealthuU.BLL.Services.Interfaces.Cache;
using HealthuU.BLL.Services.Realizations;

using HealthyU.DAL.Entities;
using HealthyU.DAL.Repositories.Interfaces.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
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
    private readonly ICacheService _cacheService;
    public ArticleService(
        IRepositoryWrapper repositoryWrapper, 
        IMapper mapper,
        IImageService imageService,
        ICacheService cacheService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _imageService = imageService;
        _cacheService = cacheService;
    }

    public async Task<Result<List<ArticleDTO>>> GetAllArticles()
    {
        var articlesDTO = await _cacheService.GetOrSetAsync(
            $"{nameof(ArticleService)}|GetAllArticles",
            async () =>
            {
                var articles = await _repositoryWrapper
                    .ArticleRepository
                    .GetAllAsync(
                        predicate: a => a.IsPublished,
                        include: q => q.Include(x => x.Image)
                    );
                return _mapper.Map<List<ArticleDTO>>(articles);
            }
        );
        return Result.Success(articlesDTO);
    }

    public async Task<Result<List<ArticleDTO>>> GetAllArticlesWithoutSelected(int id)
    {
        var articlesDTO = await _cacheService.GetOrSetAsync(
                $"{nameof(ArticleService)}|GetAllArticlesWithoutSelected|{id}",
                async () =>
                {
                    var articles = await _repositoryWrapper.ArticleRepository.GetAllAsync(
                        predicate: a => a.Id != id,
                        include: q => q.Include(x => x.Image)
                    );
                    return _mapper.Map<List<ArticleDTO>>(articles);
                }
            );
        return Result.Success(articlesDTO);
    }

    public async Task<Result<List<ArticleDTO>>> GetUnpublishedArticles()
    {
        var articlesDTO = await _cacheService.GetOrSetAsync(
                $"{nameof(ArticleService)}|GetUnpublishedArticles",
                async () =>
                {
                    var articles = await _repositoryWrapper.ArticleRepository.GetAllAsync(
                        predicate: a => !a.IsPublished,
                        include: q => q.Include(x => x.Image)
                    );
                    return _mapper.Map<List<ArticleDTO>>(articles);
                }
            );
        return Result.Success(articlesDTO);
    }

    public async Task<Result<ArticleDTO>> GetArticleById(int articleId)
    {
        var articlesDTO = await _cacheService.GetOrSetAsync(
                $"{nameof(ArticleService)}|GetArticleById|{articleId}",
                async () =>
                {
                    var article = await _repositoryWrapper.ArticleRepository.GetFirstOrDefaultAsync(
                        predicate: a => a.Id == articleId,
                        include: q => q.Include(x => x.Image)
                    );
                    return _mapper.Map<ArticleDTO>(article);
                }
            );
        return Result.Success(articlesDTO);
    }
    public async Task<Result<List<ArticleDTO>>> GetArticlesByUserId(int userId)
    {
        var articlesDTO = await _cacheService.GetOrSetAsync(
                $"{nameof(ArticleService)}|GetArticlesByUserId|{userId}",
                async () =>
                {
                    var articles = await _repositoryWrapper.ArticleRepository.GetAllAsync(
                        predicate: a => a.UserId == userId,
                        include: q => q.Include(x => x.Image)
                    );
                    return _mapper.Map<List<ArticleDTO>>(articles);
                }
            );
        return Result.Success(articlesDTO);
    }

    public async Task<Result> ImportArticlesToJsonAsync()
    {
        var articles = await _repositoryWrapper.ArticleRepository.GetAllAsync(include: a => a.Include(x => x.Image)!);
        var articlesDTO = _mapper.Map<List<ArticleDTO>>(articles);

        string jsonString = System.Text.Json.JsonSerializer.Serialize(articlesDTO, new JsonSerializerOptions { WriteIndented = true });

        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(desktopPath, "articles.json");

        await File.WriteAllTextAsync(filePath, jsonString);
        return Result.Success();
    }

    public async Task<Result<ArticleDTO>> CreateArticleWithTransaction(ArticleDTO articleDTO)
    {
        using var transaction = await _repositoryWrapper.BeginTransactionAsync();

        try
        {
            if (articleDTO.Image != null)
            {
                var imageDTO = await _imageService.CreateOrUpdateImageAsync(articleDTO.Image);
                articleDTO.ImageId = imageDTO.Id;
            }

            var article = _mapper.Map<Article>(articleDTO);
            _repositoryWrapper.ArticleRepository.Create(article);
            await _repositoryWrapper.SaveChangesAsync();

            _cacheService.Invalidate($"{nameof(ArticleService)}|GetAllArticles");

            await transaction.CommitAsync();
            return Result.Success(_mapper.Map<ArticleDTO>(article));
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return Result.Failure<ArticleDTO>($"Failed to create article: {ex.Message}");
        }
    }

    public async Task<Result<ArticleDTO>> CreateArticle(ArticleDTO articleDTO)
    {
        try
        {
            if (articleDTO.Image != null)
            {
                var imageDTO = await _imageService.CreateOrUpdateImageAsync(articleDTO.Image);
                articleDTO.ImageId = imageDTO.Id;
            }

            var article = _mapper.Map<Article>(articleDTO);
            _repositoryWrapper.ArticleRepository.Create(article);
            await _repositoryWrapper.SaveChangesAsync();

            _cacheService.Invalidate($"{nameof(ArticleService)}|GetAllArticles");

            return Result.Success(_mapper.Map<ArticleDTO>(article));
        }
        catch (Exception ex)
        {
            return Result.Failure<ArticleDTO>($"Failed to create article: {ex.Message}");
        }
    }

    public async Task<Result> ImportArticlesFromJsonFileAsync()
    {
        using var transaction = await _repositoryWrapper.BeginTransactionAsync();

        try
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

            _cacheService.Invalidate("ArticleService_GetAllArticles");
            _cacheService.Invalidate("ArticleService_GetUnpublishedArticles");

            await transaction.CommitAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return Result.Failure($"Import failed: {ex.Message}");
        }
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

        _cacheService.Invalidate($"{nameof(ArticleService)}|GetAllArticles");
        _cacheService.Invalidate($"{nameof(ArticleService)}|GetArticleById|{articleId}");
        
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
        try
        {
            await _repositoryWrapper.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result.Failure<ArticleDTO>("The article was modified by another user.");
        }

        _cacheService.Invalidate($"{nameof(ArticleService)}|GetAllArticles");
        _cacheService.Invalidate($"{nameof(ArticleService)}|GetArticleById|{articleId}");
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

        _cacheService.Invalidate($"{nameof(ArticleService)}|GetAllArticles");
        _cacheService.Invalidate($"{nameof(ArticleService)}|GetUnpublishedArticles");
        _cacheService.Invalidate($"{nameof(ArticleService)}|GetArticleById|{id}");
        return Result.Success(_mapper.Map<ArticleDTO>(article));
    }
}
