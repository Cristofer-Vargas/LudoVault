using LudoVault.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LudoVault.Mappings
{
  public class UserMap : IEntityTypeConfiguration<UserModel>
  {
    public void Configure(EntityTypeBuilder<UserModel> builder)
    {
      builder.ToTable("user");

      builder.Property(p => p.Name)
        .HasColumnName("name")
        .HasMaxLength(100)
        .IsUnicode(false)
        .IsRequired(true);

      builder.Property(p => p.Email)
        .HasColumnName("email")
        .HasMaxLength(100)
        .IsUnicode(false)
        .IsRequired(true);

      builder.Property(p => p.AvatarUrl)
        .HasColumnName("avatar_url")
        .HasMaxLength(255)
        .IsUnicode(false)
        .IsRequired(true);

      builder.Property(p => p.Bio)
        .HasColumnName("bio")
        .HasMaxLength(1200)
        .IsUnicode(true)
        .IsRequired(false);

      builder.Property(p => p.PasswordHash)
        .HasColumnName("password_hash")
        .HasMaxLength(255)
        .IsUnicode(false)
        .IsRequired(true);

      builder.Property(p => p.CreatedAt)
        .HasColumnName("created_at")
        .HasColumnType("DATETIME")
        .IsRequired(true);

      builder.HasData(
          new UserModel
          {
            Id = 1,
            Name = "Cristofer",
            Email = "cristof@gmail.com",
            AvatarUrl = "/uploads/users/default-image.webp",
            Bio = "Bem vindos ao meu perfil!",
            PasswordHash = "$2a$12$Ih3ZGAGY3KX9AQ6hJFfhL.NdBsGN0TonaaIPigkVBpb.VDN9aXuj2",
            CreatedAt = new System.DateTime(2024, 1, 1, 0, 0, 0)
          }
        );
    }
  }
}
