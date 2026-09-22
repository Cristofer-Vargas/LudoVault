using LudoVault.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LudoVault.Mappings
{
  public class DeveloperGameMap : IEntityTypeConfiguration<DeveloperGameModel>
  {
    public void Configure(EntityTypeBuilder<DeveloperGameModel> builder)
    {
      builder.ToTable("developer_game");

      builder.HasIndex(d => new { d.GameId, d.DeveloperId });

      builder.Property(d => d.GameId)
        .HasColumnName("game_id")
        .IsRequired();

      builder.Property(d => d.DeveloperId)
        .HasColumnName("developer_id")
        .IsRequired();

      builder.HasData(
        new DeveloperGameModel { Id = 1, DeveloperId = 2, GameId = 1 }
      );
    }
  }
}