using LudoVault.Data;
using LudoVault.Model;
using LudoVault.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LudoVault.Repositories
{
  public class UserRepository(MysqlContext dbContext) : IUserRepository
  {
    public readonly MysqlContext _dbContext = dbContext;

    // Usuário
    public async Task<UserModel>? AtualizarUsuarioAsync(UserModel userUpdate, int id)
    {
      var userExisted = await BuscarUsuarioPorIdAsync(id);
      _dbContext.Users.Entry(userExisted).CurrentValues.SetValues(userUpdate);
      await _dbContext.SaveChangesAsync();

      return userExisted;
    }
    public async Task<UserModel>? CriarUsuarioAsync(UserModel user)
    {
      await _dbContext.Users.AddAsync(user);
      await _dbContext.SaveChangesAsync();
      return user;
    }
    public async Task<UserModel>? BuscarUsuarioPorIdAsync(int id)
    {
      return await _dbContext.Users.FindAsync(id);
    }
    public async Task<bool> VerificarEmailExistenteAsync(int? userId, string email)
    {
      if (userId != null)
      {
        return await _dbContext.Users.AnyAsync(u => u.Id != userId && u.Email == email);
      }

      return await _dbContext.Users.AnyAsync(u => u.Email == email);
    }
    public async Task<bool> AtualizarImagemDePerfilAsync(UserModel user)
    {
      _dbContext.Users
        .Update(user);
      await _dbContext.SaveChangesAsync();
      return true;
    }
    public async Task<bool> DeletarUsuarioAsync(UserModel user)
    {
      await _dbContext.Users
        .Where(u => u.Id == user.Id)
        .ExecuteDeleteAsync();
      return true;

    }

    // Listas de Usuário
    public async Task<UserListModel>? CriarUserListAsync(UserListModel userList)
    {
      await _dbContext.UserLists.AddAsync(userList);
      await _dbContext.SaveChangesAsync();

      return await _dbContext.UserLists
              .AsNoTracking()
              .FirstOrDefaultAsync(ul => ul.Id == userList.Id);
    }
    public async Task<UserListModel>? AtualizarUserListAsync(UserListModel userList, int userId, int listId)
    {
      var currentUserList = await _dbContext.UserLists.FindAsync(listId);
      if (currentUserList == null) return currentUserList;

      userList.Id = currentUserList.Id;
      userList.CreatedAt = currentUserList.CreatedAt;
      userList.UserId = userId;

      _dbContext.UserLists.Entry(currentUserList).CurrentValues.SetValues(userList);
      await _dbContext.SaveChangesAsync();

      return await _dbContext.UserLists
              .Include(ul => ul.ListItems)
                      .ThenInclude(lg => lg.Game)
                              .ThenInclude(g => g.Publisher)
              .FirstOrDefaultAsync(ul => ul.Id == listId);
    }
    public async Task<UserListModel>? AdicionarJogoAListaAsync(UserListGameModel userGameList)
    {
      await _dbContext.UserListsItems.AddAsync(userGameList);
      await _dbContext.SaveChangesAsync();

      return await _dbContext.UserLists
              .Include(ul => ul.ListItems)
                      .ThenInclude(lg => lg.Game)
                              .ThenInclude(g => g.Publisher)
              .FirstOrDefaultAsync(ul => ul.Id == userGameList.ListId);
    }
    public async Task<UserListGameModel>? BuscarItemDaListaAsync(int userListGameId)
    {
      return await _dbContext.UserListsItems.FindAsync(userListGameId);
    }
    public async Task<List<UserListModel>> BuscarUserListsPorUsuarioAsync(int userId)
    {
      return await _dbContext.UserLists
              .AsNoTracking()
              .Include(ul => ul.ListItems)
                      .ThenInclude(uli => uli.Game)
                              .ThenInclude(g => g.Publisher)
              .Where(ul => ul.UserId == userId)
              .AsSplitQuery()
              .ToListAsync();
    }
    public async Task<bool> ExisteListaComMesmoNomeAsync(string name, int userId)
    {
      return await _dbContext.UserLists.AnyAsync(ul => ul.Name == name && ul.UserId == userId);
    }
    public async Task<bool> ExisteUserListAsync(int listId, int userId)
    {
      return await _dbContext.UserLists.AnyAsync(ul => ul.Id == listId && ul.UserId == userId);
    }
    public async Task<bool> JogoExisteNaListaAsync(int gameId, int listId)
    {
      return await _dbContext.UserListsItems.AnyAsync(uli => uli.GameId == gameId && uli.ListId == listId);
    }
    public async Task<bool> DeletarUserListAsync(int listId)
    {
      var userListToDelete = await _dbContext.UserLists.FindAsync(listId);
      if (userListToDelete == null) return false;

      _dbContext.UserLists.Remove(userListToDelete);
      await _dbContext.SaveChangesAsync();
      return true;
    }
    public async Task<bool> RemoverJogoDaListaAsync(int listId, int gameId)
    {
      var gameToDelete = await _dbContext.UserListsItems
              .FirstOrDefaultAsync(uli => uli.ListId == listId && uli.GameId == gameId);

      if (gameToDelete == null) return false;

      _dbContext.UserListsItems.Remove(gameToDelete);
      await _dbContext.SaveChangesAsync();

      return true;
    }

    //Biblioteca de Usuário
    public async Task<bool> AdicionarJogoABibliotecaAsync(UserLibraryModel userLibraryGame)
    {
      await _dbContext.UserLibrary
              .AddAsync(userLibraryGame);

      await _dbContext.SaveChangesAsync();

      return true;

    }
    public async Task<List<UserLibraryModel>> BuscarJogosDaBibliotecaAsync(int id)
    {
      var jogosLibrary = await _dbContext.UserLibrary
              .Include(ul => ul.Game)
                      .ThenInclude(g => g.Publisher)
              .Include(ul => ul.User)
              .Where(ul => ul.UserId == id)
              .ToListAsync();

      return jogosLibrary;
    }
    public async Task<bool> RemoverJogoDaBibliotecaAsync(int libraryId)
    {
      var userLibrary = await _dbContext.UserLibrary.FindAsync(libraryId);
      if (userLibrary == null) return false;

      _dbContext.UserLibrary.Remove(userLibrary);
      await _dbContext.SaveChangesAsync();

      return true;
    }
    public async Task<bool> ExisteJogoNaBibliotecaAsync(int userId, int gameId)
    {
      var exist = await _dbContext.UserLibrary
              .FirstOrDefaultAsync(ul => ul.UserId == userId && ul.GameId == gameId);

      if (exist == null) return false;

      return true;
    }

    // Avaliações de Usuário
    public async Task<List<RatingModel>> BuscarAvaliacoesDoUsuarioAsync(int userId)
    {
      return await _dbContext.Ratings
              .Include(gr => gr.Game)
              .Include(gr => gr.User)
              .Where(gr => gr.UserId == userId)
              .ToListAsync();
    }
    public async Task<List<RatingModel>> AdicionarUserRatingAsync(RatingModel rating)
    {
      await _dbContext.Ratings.AddAsync(rating);
      await _dbContext.SaveChangesAsync();

      return await _dbContext.Ratings
        .Include(r => r.Game)
        .Include(r => r.User)
        .Where(r => r.UserId == rating.UserId)
        .ToListAsync();
    }

    public async Task<RatingModel>? BuscarRatingPorUserEGameAsync(int userId, int gameId)
    {
      var rating = await _dbContext.Ratings
        .Where(r => r.UserId == userId && r.GameId == gameId)
        .FirstOrDefaultAsync();

      return rating;
    }

    public async Task<RatingModel>? BuscarRatingPorIdAsync(int ratingId)
    {
      return await _dbContext.Ratings
        .Include(r => r.Game)
        .Include(r => r.User)
        .Where(r => r.Id == ratingId)
        .FirstOrDefaultAsync();
    }

    public async Task<RatingModel> AtualizarRatingPorIdAsync(RatingModel rating, int ratingId)
    {
      var currentRating = await _dbContext.Ratings.FindAsync(ratingId);
      currentRating.Rating = rating.Rating;
      currentRating.Comment = rating.Comment;

      _dbContext.Ratings.Update(currentRating);
      await _dbContext.SaveChangesAsync();

      return await _dbContext.Ratings
        .Include(r => r.Game)
        .Include(r => r.User)
        .Where(r => r.Id == currentRating.Id)
        .FirstOrDefaultAsync();
    }

    public async Task<bool> RemoverRatingAsync(RatingModel rating)
    {
      _dbContext.Ratings.Remove(rating);
      await _dbContext.SaveChangesAsync();

      return true;
    }
  }
}
