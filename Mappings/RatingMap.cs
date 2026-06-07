using LudoVault.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LudoVault.Mappings
{
  public class RatingMap : IEntityTypeConfiguration<RatingModel>
  {
    public void Configure(EntityTypeBuilder<RatingModel> builder)
    {
      builder.ToTable("rating");

      builder.Property(p => p.Rating)
        .HasColumnName("rating")
        .HasPrecision(2, 1)
        .IsRequired();

      builder.Property(p => p.GameId)
        .HasColumnName("game_id")
        .IsRequired();

      builder.Property(p => p.UserId)
        .HasColumnName("user_id")
        .IsRequired();

      builder.Property(p => p.Comment)
        .HasColumnName("comment")
        .HasMaxLength(1200)
        .IsUnicode(true)
        .IsRequired();

      builder.Property(p => p.CreatedAt)
        .HasColumnName("created_at")
        .HasColumnType("DATETIME")
        .IsRequired();

      builder.HasOne(p => p.Game)
        .WithMany(p => p.GameRatings)
        .HasForeignKey(p => p.GameId)
        .HasConstraintName("FK_rating_game")
        .OnDelete(DeleteBehavior.Cascade);

      builder.HasOne(p => p.User)
        .WithMany(p => p.Ratings)
        .HasForeignKey(p => p.UserId)
        .HasConstraintName("FK_rating_user")
        .OnDelete(DeleteBehavior.Cascade);

      builder.HasData(
        new RatingModel { Id = 1, GameId = 1, UserId = 1, Rating = 5.0m, Comment = "Uma verdadeira obra prima dos videogames!", CreatedAt = new System.DateTime(2024, 1, 1) }
      );
    }
  }
}