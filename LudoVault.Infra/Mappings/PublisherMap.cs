using LudoVault.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LudoVault.Infra.Mappings
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

      builder.Property(p => p.FundationAt)
        .HasColumnName("fundation_at")
        .HasColumnType("DATE")
        .IsRequired();

      builder.HasData(
        new PublisherModel { Id = 1, Name = "Rockstar Games", FundationAt = new System.DateOnly(1998, 12, 1) },
        new PublisherModel { Id = 2, Name = "Nintendo", FundationAt = new System.DateOnly(1889, 9, 1) },
        new PublisherModel { Id = 3, Name = "Sony Interactive", FundationAt = new System.DateOnly(1993, 11, 16) }
      );
    }
  }
}