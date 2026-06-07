using LudoVault.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LudoVault.Mappings
{
  public class GameMap : IEntityTypeConfiguration<GameModel>
  {
    public void Configure(EntityTypeBuilder<GameModel> builder)
    {
      builder.ToTable("game");

      builder.Property(p => p.Name)
        .HasColumnName("name")
        .HasMaxLength(200)
        .IsUnicode(true)
        .IsRequired();

      builder.Property(p => p.ImageUrl)
        .HasColumnName("image_url")
        .HasMaxLength(255)
        .IsUnicode(false)
        .IsRequired();

      builder.Property(p => p.Description)
        .HasColumnName("description")
        .HasMaxLength(1500)
        .IsUnicode(true)
        .IsRequired();

      builder.Property(p => p.PublisherId)
        .HasColumnName("publisher_id")
        .IsRequired();

      builder.Property(p => p.CreatedAt)
        .HasColumnName("created_at")
        .HasColumnType("DATETIME")
        .IsRequired();

      builder.HasOne(p => p.Publisher)
        .WithMany(p => p.Games)
        .HasForeignKey(p => p.PublisherId)
        .HasConstraintName("FK_game_publisher")
        .OnDelete(DeleteBehavior.Restrict);

      builder.HasData(
        new GameModel
        {
          Id = 1,
          Name = "Red Dead Redemption 2",
          ImageUrl = "/uploads/games/default-image.webp",
          Description = "Estados Unidos, 1899. Arthur Morgan e a gangue Van der Linde são forçados a fugir. Com agentes federais e os melhores caçadores de recompensas no seu encalço, a gangue precisa roubar, assaltar e lutar para sobreviver no impiedoso coração dos Estados Unidos. Conforme divisões internas profundas ameaçam despedaçar a gangue, Arthur deve fazer uma escolha entre os seus próprios ideais e a lealdade à gangue que o criou.",
          PublisherId = 1,
          Publisher = null!,
          CreatedAt = new System.DateTime(2024, 1, 1, 0, 0, 0)
        });
    }
  }
}
