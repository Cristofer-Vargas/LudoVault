using LudoVault.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LudoVault.Infra.Mappings
{
  public class GamePlatformMap : IEntityTypeConfiguration<GamePlatformModel>
  {
    public void Configure(EntityTypeBuilder<GamePlatformModel> builder)
    {
      builder.ToTable("game_platform");

      builder.HasIndex(p => new { p.GameId, p.PlatformId }).IsUnique();

      builder.Property(p => p.GameId)
        .HasColumnName("game_id")
        .IsRequired();

      builder.Property(p => p.PlatformId)
        .HasColumnName("platform_id")
        .IsRequired();

      builder.HasOne(p => p.Game)
        .WithMany(p => p.GamePlatforms) 
        .HasForeignKey(p => p.GameId)
        .HasConstraintName("FK_game_platform_game")
        .OnDelete(DeleteBehavior.Cascade);

      builder.HasOne(p => p.Platform)
        .WithMany(p => p.GamePlatforms)
        .HasForeignKey(p => p.PlatformId)
        .HasConstraintName("FK_game_platform_platform")
        .OnDelete(DeleteBehavior.Cascade);

      builder.HasData(
        new GamePlatformModel { Id = 1, GameId = 1, PlatformId = 1, Game = null!, Platform = null! },
        new GamePlatformModel { Id = 2, GameId = 1, PlatformId = 2, Game = null!, Platform = null! },
        new GamePlatformModel { Id = 3, GameId = 1, PlatformId = 3, Game = null!, Platform = null! }
      );
    }
  }
}