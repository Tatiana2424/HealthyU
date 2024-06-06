using CSharpFunctionalExtensions;

using HealthuU.BLL.DTO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.Services.Interfaces;

public interface IArticleService
{
    Task<Result<List<ArticleDTO>>> GetAllArticles();
    Task<Result<ArticleDTO>> GetArticleById(int articleId);
    Task<Result<List<ArticleDTO>>> GetArticlesByUserId(int userId);
    Task<Result<ArticleDTO>> CreateArticle(ArticleDTO articleDTO);
    Task<Result<bool>> DeleteArticle(int articleId);
    Task<Result<ArticleDTO>> UpdateArticle(int articleId, ArticleDTO articleDTO);
    Task<Result<List<ArticleDTO>>> GetAllArticlesWithoutSelected(int id);
    Task<Result> ImportArticlesToJsonAsync();
    Task<Result> ImportArticlesFromJsonFileAsync();
    Task<Result<List<ArticleDTO>>> GetUnpublishedArticles();
    Task<Result<ArticleDTO>> PublishArticle(int id, bool isPublish);
}
