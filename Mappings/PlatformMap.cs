using LudoVault.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LudoVault.Mappings
{
  public class PlatformMap : IEntityTypeConfiguration<PlatformModel>
  {
    public void Configure(EntityTypeBuilder<PlatformModel> builder)
    {
      builder.ToTable("platform");

      builder.Property(p => p.Name)
        .HasColumnName("name")
        .HasMaxLength(30)
        .IsUnicode(false)
        .IsRequired();

      builder.HasData(
        new PlatformModel { Id = 1, Name = "PlayStation 4" },
        new PlatformModel { Id = 2, Name = "Xbox One" },
        new PlatformModel { Id = 3, Name = "PC" }
      );
    }
  }
}