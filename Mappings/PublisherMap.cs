using LudoVault.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LudoVault.Mappings
{
  public class PublisherMap : IEntityTypeConfiguration<PublisherModel>
  {
    public void Configure(EntityTypeBuilder<PublisherModel> builder)
    {
      builder.ToTable("publisher");

      builder.Property(p => p.Name)
        .HasColumnName("name")
        .HasMaxLength(60)
        .IsUnicode(false)
        .IsRequired();

      builder.HasData(
        new PublisherModel { Id = 1, Name = "Rockstar Games" },
        new PublisherModel { Id = 2, Name = "Nintendo" },
        new PublisherModel { Id = 3, Name = "Sony Interactive" }
      );
    }
  }
}