using LudoVault.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LudoVault.Infra.Mappings
{
  public class GameGenreMap : IEntityTypeConfiguration<GameGenreModel>
  {
    public void Configure(EntityTypeBuilder<GameGenreModel> builder)
    {
      builder.ToTable("game_genre");

      builder.HasIndex(p => new { p.GameId, p.GenreId }).IsUnique();

      builder.Property(p => p.GameId)
        .HasColumnName("game_id")
        .IsRequired();

      builder.Property(p => p.GenreId)
        .HasColumnName("genre_id")
        .IsRequired();

      builder.HasOne(p => p.Game)
        .WithMany(p => p.GameGenres)
        .HasForeignKey(p => p.GameId)
        .HasConstraintName("FK_game_genre_game")
        .OnDelete(DeleteBehavior.Cascade);

      builder.HasOne(p => p.Genre)
        .WithMany(p => p.GameGenres)
        .HasForeignKey(p => p.GenreId)
        .HasConstraintName("FK_game_genre_genre")
        .OnDelete(DeleteBehavior.Cascade);

      builder.HasData(
        new GameGenreModel { Id = 1, GameId = 1, GenreId = 1 }, // Ação
        new GameGenreModel { Id = 2, GameId = 1, GenreId = 2 }, // Aventura
        new GameGenreModel { Id = 3, GameId = 1, GenreId = 4 }  // Mundo Aberto
      );
    }
  }
}
