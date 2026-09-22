using LudoVault.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LudoVault.Mappings
{
  public class PublisherGameMap : IEntityTypeConfiguration<PublisherGameModel>
  {
    public void Configure(EntityTypeBuilder<PublisherGameModel> builder)
    {
      builder.ToTable("publisher_game");

      builder.HasIndex(p => new { p.PublisherId, p.GameId });

      builder.Property(p => p.GameId)
        .HasColumnName("game_id")
        .IsRequired();

      builder.Property(p => p.PublisherId)
        .HasColumnName("publisher_id")
        .IsRequired();

      builder.HasOne(pg => pg.Publisher)
        .WithMany(p => p.PublisherGame)
        .HasForeignKey(pg => pg.PublisherId)
        .HasConstraintName("FK_publisher_game_publisher")
        .OnDelete(DeleteBehavior.Restrict);

      builder.HasOne(pg => pg.Game)
        .WithMany(g => g.PublisherGame)
        .HasForeignKey(pg => pg.GameId)
        .HasConstraintName("FK_publisher_game_game")
        .OnDelete(DeleteBehavior.Cascade);

      builder.HasData(
        new PublisherGameModel { Id = 1, GameId = 1, PublisherId = 4, Game = null!, Publisher = null! },
        new PublisherGameModel { Id = 1, GameId = 1, PublisherId = 1, Game = null!, Publisher = null! }
      );
    }
  }
}