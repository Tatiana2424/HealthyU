using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Collections.Specialized.BitVector32;

namespace HealthuU.BLL.Model;

public class ApiRecipe
{
    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("original_video_url")]
    public string? OriginalVideoUrl { get; set; }

    [JsonProperty("thumbnail_url")]
    public string? ThumbnailUrl { get; set; }

    [JsonProperty("nutrition")]
    public Nutrition? Nutrition { get; set; }

    [JsonProperty("sections")]
    public List<Section>? Sections { get; set; }

    [JsonProperty("instructions")]
    public List<Instruction>? Instructions { get; set; }

    [JsonProperty("num_servings")]
    public int NumServings { get; set; }

    [JsonProperty("user_ratings")]
    public UserRatings? UserRatings { get; set; }

    [JsonProperty("keywords")]
    public string? Keywords { get; set; }

    [JsonProperty("total_time_minutes")]
    public int? TotalTimeMinutes { get; set; }

    [JsonProperty("total_time_tier")]
    public TotalTimeTier? TotalTimeTier { get; set; }
}


public class ApiResponse
{
    [JsonProperty("count")]
    public int Count { get; set; }

    [JsonProperty("results")]
    public List<ApiRecipe>? Results { get; set; }
}



public class Nutrition
{
    [JsonProperty("calories")]
    public int Calories { get; set; }

    [JsonProperty("carbohydrates")]
    public int Carbohydrates { get; set; }

    [JsonProperty("fat")]
    public int Fat { get; set; }

    [JsonProperty("fiber")]
    public int Fiber { get; set; }

    [JsonProperty("protein")]
    public int Protein { get; set; }
}


public class Section
{
    [JsonProperty("components")]
    public List<Component>? Components { get; set; }
}

public class Component
{
    [JsonProperty("raw_text")]
    public string? RawText { get; set; }

    [JsonProperty("position")]
    public int Position { get; set; }
}
public class Instruction
{
    [JsonProperty("display_text")]
    public string? DisplayText { get; set; }

    [JsonProperty("position")]
    public int Position { get; set; }
}

public class UserRatings
{
    [JsonProperty("count_positive")]
    public int CountPositive { get; set; }

    [JsonProperty("count_negative")]
    public int CountNegative { get; set; }

    [JsonProperty("score")]
    public double Score { get; set; }
}

public class TotalTimeTier
{
    [JsonProperty("display_tier")]
    public string? DisplayTier { get; set; }
}
