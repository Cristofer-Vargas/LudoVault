using LudoVault.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LudoVault.Infra.Mappings
{
  public class GenreMap : IEntityTypeConfiguration<GenreModel>
  {
    public void Configure(EntityTypeBuilder<GenreModel> builder)
    {
      builder.ToTable("genre");

      builder.Property(p => p.Name)
        .HasColumnName("name")
        .HasMaxLength(20)
        .IsUnicode(false)
        .IsRequired();

      builder.HasData(
        new GenreModel { Id = 1, Name = "Ação" },
        new GenreModel { Id = 2, Name = "Aventura" },
        new GenreModel { Id = 3, Name = "RPG" },
        new GenreModel { Id = 4, Name = "Mundo Aberto" }
      );
    }
  }
}
