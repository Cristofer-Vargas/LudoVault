using LudoVault.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LudoVault.Mappings {
  public class DeveloperMap : IEntityTypeConfiguration<DeveloperModel>
  {
    public void Configure(EntityTypeBuilder<DeveloperModel> builder)
    {
      builder.ToTable("developer");

      builder.Property(d => d.Name)
        .HasColumnName("name")
        .HasMaxLength(60)
        .IsUnicode(false)
        .IsRequired();

      builder.Property(d => d.FundationAt)
        .HasColumnName("fundation_at")
        .HasColumnType("DATE")
        .IsRequired();

      builder.HasData(
        new DeveloperModel{ Id = 1, Name = "Rockstar North", FundationAt = new DateOnly(2001, 12, 31) },
        new DeveloperModel{ Id = 2, Name = "Rockstar Games", FundationAt = new DateOnly(1998, 12, 01) }
      );
    }
  }
}