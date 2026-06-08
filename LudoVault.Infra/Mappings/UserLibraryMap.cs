using LudoVault.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LudoVault.Infra.Mappings
{
  public class UserLibraryMap : IEntityTypeConfiguration<UserLibraryModel>
  {
    public void Configure(EntityTypeBuilder<UserLibraryModel> builder)
    {
      builder.ToTable("user_library");

      builder.HasIndex(p => new { p.UserId, p.GameId }).IsUnique();

      builder.Property(p => p.UserId)
        .HasColumnName("user_id")
        .IsRequired();

      builder.Property(p => p.GameId)
        .HasColumnName("game_id")
        .IsRequired();

      builder.Property(p => p.AddedAt)
        .HasColumnName("added_at")
        .HasColumnType("DATETIME")
        .IsRequired();

      builder.HasOne(p => p.User)
        .WithMany(p => p.UserLibraries)
        .HasForeignKey(p => p.UserId)
        .HasConstraintName("FK_user_library_user")
        .OnDelete(DeleteBehavior.Cascade);

      builder.HasOne(p => p.Game)
        .WithMany(p => p.UserLibraries)
        .HasForeignKey(p => p.GameId)
        .HasConstraintName("FK_user_library_game")
        .OnDelete(DeleteBehavior.Restrict);

      builder.HasData(
        new UserLibraryModel { Id = 1, UserId = 1, GameId = 1, AddedAt = new System.DateTime(2024, 1, 1) }
      );
    }
  }
}