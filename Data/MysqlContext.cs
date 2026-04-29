using LudoVault.Model;
using Microsoft.EntityFrameworkCore;

namespace LudoVault.Data
{
    public class MysqlContext : DbContext
    {
        public MysqlContext(DbContextOptions<MysqlContext> options) : base(options) { }

        public DbSet<GameModel> Games { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<PublisherModel> Publishers { get; set; }
        public DbSet<GamePlatformModel> GamePlatforms { get; set; }
        public DbSet<GameGenreModel> GameGenres { get; set; }
        public DbSet<PlatformModel> Platforms { get; set; }
        public DbSet<GenreModel> Genres { get; set; }
        public DbSet<GameRatingModel> GameRatings { get; set; }
        public DbSet<UserListModel> UserLists { get; set; }
        public DbSet<UserListItemsModel> UserListsItems { get; set; }
    }
}
