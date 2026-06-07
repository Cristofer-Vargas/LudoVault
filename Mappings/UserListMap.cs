using LudoVault.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LudoVault.Mappings
{
  public class UserListMap : IEntityTypeConfiguration<UserListModel>
  {
    public void Configure(EntityTypeBuilder<UserListModel> builder)
    {
      builder.ToTable("user_list");

      builder.Property(p => p.Name)
        .HasColumnName("name")
        .HasMaxLength(60)
        .IsUnicode(true)
        .IsRequired();

      builder.Property(p => p.UserId)
        .HasColumnName("user_id")
        .IsRequired();

      builder.Property(p => p.CreatedAt)
        .HasColumnName("created_at")
        .HasColumnType("DATETIME")
        .IsRequired();

      builder.HasOne(p => p.User)
        .WithMany(p => p.Lists)
        .HasForeignKey(p => p.UserId)
        .HasConstraintName("FK_user_list_user")
        .OnDelete(DeleteBehavior.Cascade);

      builder.HasData(
        new UserListModel { Id = 1, UserId = 1, Name = "Meus Jogos Favoritos", CreatedAt = new System.DateTime(2024, 1, 1) }
      );
    }
  }
}