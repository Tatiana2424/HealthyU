using HealthyU.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class HealthyUDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<RecipeNutrition> RecipeNutritions { get; set; }
    public DbSet<RecipeUserRating> RecipeUserRatings { get; set; }
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
    public DbSet<RecipeInstruction> RecipeInstructions { get; set; }
    public DbSet<RecipeTimeInfo> RecipeTimeInfos { get; set; }
    public DbSet<SearchKeyword> SearchKeywords { get; set; }
    public DbSet<RecipeSearchKeyword> RecipeSearchKeywords { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<BMIData> BMIData { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public HealthyUDbContext(DbContextOptions<HealthyUDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Recipe>()
            .HasOne(r => r.Image)
            .WithMany()
            .HasForeignKey(r => r.ImageId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Recipe>()
            .HasOne(r => r.User)
            .WithMany(u => u.Recipes)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RecipeSearchKeyword>()
            .HasKey(rsk => new { rsk.RecipeId, rsk.KeywordId });

        modelBuilder.Entity<RecipeSearchKeyword>()
            .HasOne(rsk => rsk.Recipe)
            .WithMany(r => r.RecipeSearchKeywords)
            .HasForeignKey(rsk => rsk.RecipeId);

        modelBuilder.Entity<RecipeSearchKeyword>()
            .HasOne(rsk => rsk.SearchKeyword)
            .WithMany(sk => sk.RecipeSearchKeywords)
            .HasForeignKey(rsk => rsk.KeywordId);

        modelBuilder.Entity<Article>()
            .HasOne(b => b.Image)
            .WithMany()
            .HasForeignKey(b => b.ImageId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Image)
            .WithMany()
            .HasForeignKey(u => u.ImageId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
