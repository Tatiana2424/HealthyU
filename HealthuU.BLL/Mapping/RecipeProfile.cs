using AutoMapper;
using BarberShop.BLL.DTO;

using HealthuU.BLL.DTO;
using HealthyU.DAL.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.Mapping;

public class RecipeProfile : Profile
{
    public RecipeProfile()
    {
        CreateMap<RecipeDTO, Recipe>()
            //.ForMember(dest => dest.Image, opt => opt.Condition(src => src.ImageId.HasValue))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image != null ? src.Image : null))
            //.ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.ImageId.HasValue ? new Image { Id = src.ImageId.Value } : null))
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.RecipeNutrition, opt => opt.MapFrom(src => src.RecipeNutrition))
            .ForMember(dest => dest.RecipeUserRating, opt => opt.MapFrom(src => src.RecipeUserRating))
            .ForMember(dest => dest.TimeInfo, opt => opt.MapFrom(src => src.TimeInfo))
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients))
            .ForMember(dest => dest.Instructions, opt => opt.MapFrom(src => src.Instructions))
            .ForMember(dest => dest.RecipeSearchKeywords, opt => opt.MapFrom(src => src.SearchKeywords.Select(keyword => new RecipeSearchKeyword { SearchKeyword = new SearchKeyword { Keyword = keyword.Keyword } })))
            //.ForMember(dest => dest.RecipeSearchKeywords, opt => opt.MapFrom(src => src.SearchKeywords.Select(keyword))
            .AfterMap((src, dest) =>
            {
                // Закріпити RecipeSearchKeywords за Recipe
                foreach (var rsk in dest.RecipeSearchKeywords)
                {
                    rsk.RecipeId = dest.Id;
                }
            })
            .ReverseMap();

        // Мапінги для інших DTO
        CreateMap<RecipeIngredientDTO, RecipeIngredient>()
            .ForMember(dest => dest.Recipe, opt => opt.Ignore());

        CreateMap<RecipeInstructionDTO, RecipeInstruction>()
            .ForMember(dest => dest.Recipe, opt => opt.Ignore());

        CreateMap<RecipeNutritionDTO, RecipeNutrition>()
            .ForMember(dest => dest.Recipe, opt => opt.Ignore());

        CreateMap<RecipeTimeInfoDTO, RecipeTimeInfo>()
            .ForMember(dest => dest.Recipe, opt => opt.Ignore());

        CreateMap<RecipeUserRatingDTO, RecipeUserRating>()
            .ForMember(dest => dest.Recipe, opt => opt.Ignore());

        CreateMap<RecipeUserRating, RecipeUserRatingDTO>()
            .ForMember(dest => dest.CountPositive, opt => opt.MapFrom(src => src.CountPositive))
            .ForMember(dest => dest.CountNegative, opt => opt.MapFrom(src => src.CountNegative))
            .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
            .ReverseMap();

        CreateMap<RecipeNutrition, RecipeNutritionDTO>()
            .ForMember(dest => dest.Protein, opt => opt.MapFrom(src => src.Protein))
            .ForMember(dest => dest.Fat, opt => opt.MapFrom(src => src.Fat))
            .ForMember(dest => dest.Calories, opt => opt.MapFrom(src => src.Calories))
            .ForMember(dest => dest.Carbohydrates, opt => opt.MapFrom(src => src.Carbohydrates))
            .ForMember(dest => dest.Fiber, opt => opt.MapFrom(src => src.Fiber))
            .ReverseMap();

        CreateMap<RecipeTimeInfo, RecipeTimeInfoDTO>()
            .ForMember(dest => dest.PrepTime, opt => opt.MapFrom(src => src.PrepTime))
            .ForMember(dest => dest.CookTime, opt => opt.MapFrom(src => src.CookTime))
            .ForMember(dest => dest.CoolTime, opt => opt.MapFrom(src => src.CoolTime))
            .ForMember(dest => dest.RestTime, opt => opt.MapFrom(src => src.RestTime))
            .ForMember(dest => dest.TotalTime, opt => opt.MapFrom(src => src.TotalTime))
            .ForMember(dest => dest.Servings, opt => opt.MapFrom(src => src.Servings))
            .ReverseMap();

        CreateMap<RecipeInstruction, RecipeInstructionDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.DisplayText, opt => opt.MapFrom(src => src.DisplayText))
            .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
            .ReverseMap();

        CreateMap<RecipeIngredient, RecipeIngredientDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
            .ReverseMap();

        CreateMap<SearchKeywordDTO, SearchKeyword>();

        CreateMap<RecipeNutrition, RecipeNutritionDTO>().ReverseMap();
        CreateMap<RecipeUserRating, RecipeUserRatingDTO>().ReverseMap();
        CreateMap<RecipeTimeInfo, RecipeTimeInfoDTO>().ReverseMap();
        CreateMap<RecipeInstruction, RecipeInstructionDTO>().ReverseMap();
        CreateMap<RecipeIngredient, RecipeIngredientDTO>().ReverseMap();

        //CreateMap<Recipe, RecipeDTO>()
        // .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image != null ? new ImageDTO { Id = src.Image.Id, Url = src.Image.Url } : null))
        // .ForMember(dest => dest.User, opt => opt.Ignore())
        // .ForMember(dest => dest.RecipeNutrition, opt => opt.MapFrom(src => src.RecipeNutrition))
        // .ForMember(dest => dest.RecipeUserRating, opt => opt.MapFrom(src => src.RecipeUserRating))
        // .ForMember(dest => dest.TimeInfo, opt => opt.MapFrom(src => src.TimeInfo))
        // .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients))
        // .ForMember(dest => dest.Instructions, opt => opt.MapFrom(src => src.Instructions))
        // .ForMember(dest => dest.SearchKeywords, opt => opt.MapFrom(src => src.RecipeSearchKeywords.Select(rsk => new SearchKeywordDTO { Id = rsk.SearchKeyword.Id, Keyword = rsk.SearchKeyword.Keyword })));
        // }

        //// Зворотній мапінг, якщо потрібно
        CreateMap<Recipe, RecipeDTO>()
                .ForMember(dest => dest.SearchKeywords, opt => opt.MapFrom(src => src.RecipeSearchKeywords.Select(rsk => new SearchKeywordDTO { Keyword = rsk.SearchKeyword.Keyword })));
   
    }
}
