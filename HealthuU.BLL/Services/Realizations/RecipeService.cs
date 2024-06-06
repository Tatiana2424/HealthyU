using AutoMapper;

using CSharpFunctionalExtensions;

using HealthuU.BLL.DTO;
using HealthuU.BLL.Services.Realizations;

using HealthyU.DAL.Entities;
using HealthyU.DAL.Repositories.Interfaces.Base;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.Services.Interfaces;

public class RecipeService : IRecipeService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly IImageService _imageService;

    public RecipeService(IRepositoryWrapper repositoryWrapper, IMapper mapper, IImageService imageService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _imageService = imageService;
    }

    public async Task CreateRecipeAsync(RecipeDTO recipeDto)
    {
        if (recipeDto.Image != null)
        {
            var imageDTO = await _imageService.CreateOrUpdateImageAsync(recipeDto.Image);
            recipeDto.ImageId = imageDTO.Id;
        }

        var recipeEntity = _mapper.Map<Recipe>(recipeDto);

        if(recipeDto?.SearchKeywords is not null)
        {
            foreach (var keywordDto in recipeDto.SearchKeywords)
            {
                var existingKeyword = await _repositoryWrapper.SearchKeywordRepository.GetFirstOrDefaultAsync(a => a.Keyword == keywordDto.Keyword);
                if (existingKeyword != null)
                {
                    var keywordsToRemove = recipeEntity.RecipeSearchKeywords.Where(rsk => rsk.SearchKeyword.Keyword == existingKeyword.Keyword).FirstOrDefault();
                    if (keywordsToRemove != null)
                    {
                        recipeEntity.RecipeSearchKeywords.Remove(keywordsToRemove);
                    }

                    recipeEntity.RecipeSearchKeywords.Add(new RecipeSearchKeyword { KeywordId = existingKeyword.Id });
                }
            }
        }

        _repositoryWrapper.RecipeRepository.Create(recipeEntity);

        await _repositoryWrapper.SaveChangesAsync();
    }

    public async Task<Result<int>> CreateRecipe(RecipeDTO recipeDto)
    {
        if (recipeDto.Image != null)
        {
            var imageDTO = await _imageService.CreateOrUpdateImageAsync(recipeDto.Image);
            recipeDto.ImageId = imageDTO.Id;
        }

        var recipeEntity = _mapper.Map<Recipe>(recipeDto);

        foreach (var keywordDto in recipeDto.SearchKeywords)
        {
            var existingKeyword = await _repositoryWrapper.SearchKeywordRepository.GetFirstOrDefaultAsync(a => a.Keyword == keywordDto.Keyword);
            if (existingKeyword != null)
            {
                var keywordsToRemove = recipeEntity.RecipeSearchKeywords.Where(rsk => rsk.SearchKeyword.Keyword == existingKeyword.Keyword).FirstOrDefault();
                if (keywordsToRemove != null)
                {
                    recipeEntity.RecipeSearchKeywords.Remove(keywordsToRemove);
                }

                recipeEntity.RecipeSearchKeywords.Add(new RecipeSearchKeyword { KeywordId = existingKeyword.Id });
            }
        }

        _repositoryWrapper.RecipeRepository.Create(recipeEntity);

        await _repositoryWrapper.SaveChangesAsync();
        return Result.Success(recipeEntity.Id);
    }

    public async Task<Result<List<RecipeDTO>>> GetAllBaseRecipeData()
    {
        var recipes = await _repositoryWrapper.RecipeRepository.GetAllAsync(include: a => a.Include(x => x.Image));
        var recipesDTO = _mapper.Map<List<RecipeDTO>>(recipes);
        return Result.Success(recipesDTO);
    }

    public async Task<Result<List<RecipeDTO>>> GetAllBasePublishedRecipeData()
    {
        var recipes = await _repositoryWrapper.RecipeRepository.GetAllAsync(a => a.IsPublished == true, include: a => a.Include(x => x.Image).Include(r => r.RecipeUserRating));
        var recipesDTO = _mapper.Map<List<RecipeDTO>>(recipes);
        return Result.Success(recipesDTO);
    }

    public async Task<Result<RecipeDTO>> PublishRecipe(int recipeId, bool isPublish)
    {
        var recipeEntity = await _repositoryWrapper.RecipeRepository.GetFirstOrDefaultAsync(a => a.Id == recipeId &&  a.IsPublished != isPublish);

        recipeEntity.IsPublished = isPublish;
        _repositoryWrapper.RecipeRepository.Update(recipeEntity);
        await _repositoryWrapper.SaveChangesAsync();
        var recipeDTO = _mapper.Map<RecipeDTO>(recipeEntity);
        return Result.Success(recipeDTO);
    }


    public async Task<Result<List<RecipeDTO>>> GetAll()
    {
        var recipes = await _repositoryWrapper.RecipeRepository.GetAllAsync(a => a.IsPublished == true,
            include: source => source
                .Include(r => r.Image)
                .Include(r => r.User)
                .Include(r => r.RecipeNutrition)
                .Include(r => r.RecipeUserRating)
                .Include(r => r.TimeInfo)
                .Include(r => r.Ingredients)
                .Include(r => r.Instructions)
                .Include(r => r.RecipeSearchKeywords)
                    .ThenInclude(rsk => rsk.SearchKeyword)); 

        var recipesDTO = _mapper.Map<List<RecipeDTO>>(recipes);
        return Result.Success(recipesDTO);
    }

    public async Task<Result<List<RecipeDTO>>> GetByUserId(int userId)
    {
        var recipes = await _repositoryWrapper.RecipeRepository.GetAllAsync(a => a.UserId == userId,
            include: source => source
                .Include(r => r.Image)
                .Include(r => r.User)
                .Include(r => r.RecipeNutrition)
                .Include(r => r.TimeInfo)
                .Include(r => r.Ingredients)
                .Include(r => r.Instructions)
                .Include(r => r.RecipeSearchKeywords)
                    .ThenInclude(rsk => rsk.SearchKeyword));

        var recipesDTO = _mapper.Map<List<RecipeDTO>>(recipes);
        return Result.Success(recipesDTO);
    }

    public async Task<Result<List<RecipeDTO>>> GetUnpublishedRecipes()
    {
        var recipes = await _repositoryWrapper.RecipeRepository.GetAllAsync(a => a.IsPublished == false,
            include: source => source
                .Include(r => r.Image)
                .Include(r => r.User)
                .Include(r => r.RecipeNutrition)
                .Include(r => r.TimeInfo)
                .Include(r => r.Ingredients)
                .Include(r => r.Instructions)
                .Include(r => r.RecipeSearchKeywords)
                    .ThenInclude(rsk => rsk.SearchKeyword));

        var recipesDTO = _mapper.Map<List<RecipeDTO>>(recipes);
        return Result.Success(recipesDTO);
    }

    public async Task<Result<RecipeDTO>> GetById(int recipeId)
    {
        var recipe = await _repositoryWrapper.RecipeRepository.GetFirstOrDefaultAsync(a => a.Id == recipeId,
            include: source => source
                .Include(r => r.Image)
                .Include(r => r.User)
                .Include(r => r.RecipeNutrition)
                .Include(r => r.RecipeUserRating)
                .Include(r => r.TimeInfo)
                .Include(r => r.Ingredients)
                .Include(r => r.Instructions)
                .Include(r => r.RecipeSearchKeywords)
                    .ThenInclude(rsk => rsk.SearchKeyword));

        var recipeDTO = _mapper.Map<RecipeDTO>(recipe);
        return Result.Success(recipeDTO);
    }

    public async Task<Result<List<RecipeDTO>>> GetBySearchKeyword(string searchKeyword)
    {
        var lowerCaseSearchKeyword = searchKeyword.ToLower();
        var recipes = await _repositoryWrapper.RecipeRepository.GetAllAsync(
                    predicate: r => r.Name.ToLower().Contains(lowerCaseSearchKeyword) ||
                        r.RecipeSearchKeywords.Any(rsk => rsk.SearchKeyword.Keyword.ToLower().Contains(lowerCaseSearchKeyword)),
            include: source => source
                .Include(r => r.Image)
                .Include(r => r.User)
                .Include(r => r.RecipeNutrition)
                .Include(r => r.RecipeUserRating)
                .Include(r => r.TimeInfo)
                .Include(r => r.Ingredients)
                .Include(r => r.Instructions)
                .Include(r => r.RecipeSearchKeywords)
                    .ThenInclude(rsk => rsk.SearchKeyword));

        var recipesDTO = _mapper.Map<List<RecipeDTO>>(recipes);
        return Result.Success(recipesDTO);
    }

    public async Task<Result<RecipeDTO>> UpdateRecipe(int recipeId, RecipeDTO recipeDto)
    {
        var recipeEntity = await _repositoryWrapper.RecipeRepository.GetFirstOrDefaultAsync(
            recipe => recipe.Id == recipeId,
            include: query => query.Include(r => r.RecipeSearchKeywords)
                                   .ThenInclude(rsk => rsk.SearchKeyword));

        if (recipeEntity == null)
        {
            return Result.Failure<RecipeDTO>("Recipe not found.");
        }

        var currentKeywordStrs = recipeEntity.RecipeSearchKeywords.Select(rsk => rsk.SearchKeyword.Keyword).ToList();
        var updatedKeywordStrs = recipeDto.SearchKeywords.Select(sk => sk.Keyword).Distinct().ToList();

        var keywordsToRemove = recipeEntity.RecipeSearchKeywords
            .Where(rsk => !updatedKeywordStrs.Contains(rsk.SearchKeyword.Keyword))
            .ToList();

        foreach (var keywordToRemove in keywordsToRemove)
        {
            _repositoryWrapper.RecipeSearchKeywordRepository.Delete(keywordToRemove);
            await _repositoryWrapper.SaveChangesAsync();
        }

        var keywordsToRemove1 = recipeEntity.RecipeSearchKeywords
           .Where(rsk => updatedKeywordStrs.Contains(rsk.SearchKeyword.Keyword))
           .ToList();

        foreach (var keywordToRemove in keywordsToRemove1)
        {
            recipeEntity.RecipeSearchKeywords.Remove(keywordToRemove);
        }


        var existingKeywords = await _repositoryWrapper.SearchKeywordRepository.GetAllAsync();
        foreach (var keywordStr in updatedKeywordStrs)
        {
            if (!currentKeywordStrs.Contains(keywordStr))
            {
                var keywordEntity = existingKeywords.FirstOrDefault(k => k.Keyword == keywordStr);

                if (keywordEntity == null)
                {
                    keywordEntity = new SearchKeyword { Keyword = keywordStr };
                    await _repositoryWrapper.SearchKeywordRepository.CreateAsync(keywordEntity);
                    await _repositoryWrapper.SaveChangesAsync();
                }


                if (!recipeEntity.RecipeSearchKeywords.Any(rsk => rsk.KeywordId == keywordEntity.Id))
                {
                    recipeEntity.RecipeSearchKeywords.Add(new RecipeSearchKeyword { RecipeId = recipeId, KeywordId = keywordEntity.Id });
                }
            }

        }
        _mapper.Map(recipeDto, recipeEntity);
        _repositoryWrapper.RecipeRepository.Update(recipeEntity);
        await _repositoryWrapper.SaveChangesAsync();

        var updatedRecipeDto = _mapper.Map<RecipeDTO>(recipeEntity);
        return Result.Success(updatedRecipeDto);
    }

    public async Task<Result<bool>> DeleteRecipeAsync(int recipeId)
    {
        var recipe = await _repositoryWrapper.RecipeRepository.GetFirstOrDefaultAsync(a => a.Id == recipeId,
             include: source => source
                 .Include(r => r.Image)
                 .Include(r => r.User)
                 .Include(r => r.RecipeNutrition)
                 .Include(r => r.RecipeUserRating)
                 .Include(r => r.TimeInfo)
                 .Include(r => r.Ingredients)
                 .Include(r => r.Instructions)
                 .Include(r => r.RecipeSearchKeywords)
                     .ThenInclude(rsk => rsk.SearchKeyword));

        if (recipe == null)
        {
            return Result.Failure<bool>("Recipe not found.");
        }

        _repositoryWrapper.RecipeNutritionRepository.Delete(recipe.RecipeNutrition);
        if(recipe.RecipeUserRating != null)
        {
            _repositoryWrapper.RecipeUserRatingRepository.Delete(recipe.RecipeUserRating);
        }
        
        _repositoryWrapper.RecipeTimeInfoRepository.Delete(recipe.TimeInfo);
        _repositoryWrapper.ImageRepository.Delete(recipe.Image);

        foreach (var ingredient in recipe.Ingredients)
        {
            _repositoryWrapper.RecipeIngredientRepository.Delete(ingredient);
        }
        foreach (var instruction in recipe.Instructions)
        {
            _repositoryWrapper.RecipeInstructionRepository.Delete(instruction);
        }
        foreach (var recipeSearchKeyword in recipe.RecipeSearchKeywords)
        {
            _repositoryWrapper.RecipeSearchKeywordRepository.Delete(recipeSearchKeyword);
        }
        _repositoryWrapper.RecipeRepository.Delete(recipe);

        await _repositoryWrapper.SaveChangesAsync();
        return Result.Success(true);
    }
}
