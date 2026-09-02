using LudoVault.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LudoVault.Mappings
{
  public class UserListItemMap : IEntityTypeConfiguration<UserListItemModel>
  {
    public void Configure(EntityTypeBuilder<UserListItemModel> builder)
    {
      builder.ToTable("user_list_game");

      builder.Property(p => p.ListId)
        .HasColumnName("list_id")
        .IsRequired();

      builder.Property(p => p.GameId)
        .HasColumnName("game_id")
        .IsRequired();

      builder.Property(p => p.AddedAt)
        .HasColumnName("added_at")
        .HasColumnType("DATETIME")
        .IsRequired();

      builder.HasOne(p => p.UserList)
        .WithMany(p => p.ListItems)
        .HasForeignKey(p => p.ListId)
        .HasConstraintName("FK_list_item_list")
        .OnDelete(DeleteBehavior.Cascade);

      builder.HasOne(p => p.Game)
        .WithMany(p => p.ListItems)
        .HasForeignKey(p => p.GameId)
        .HasConstraintName("FK_list_item_game")
        .OnDelete(DeleteBehavior.Restrict);

      builder.HasData(
        new UserListItemModel { Id = 1, ListId = 1, GameId = 1, AddedAt = new System.DateTime(2024, 1, 1) }
      );
    }
  }
}