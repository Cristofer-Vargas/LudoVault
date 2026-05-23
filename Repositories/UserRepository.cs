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
    public async Task<UserModel>? CriarUsuarioAsync(UserModel user)
    {
      try
      {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return user;
      }
      catch (Exception)
      {
        return null;
      }
    }
    public async Task<UserModel>? AtualizarUsuarioAsync(UserModel user)
    {
      try
      {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
        return user;
      }
      catch (Exception)
      {
        return null;
      }
    }
    public async Task<UserModel>? BuscarUsuarioPorIdAsync(int id)
    {
      try
      {
        return await _dbContext.Users.FindAsync(id);
      }
      catch (Exception)
      {
        return null;
      }
    }
    public async Task<bool> VerificarEmailExistenteAsync(int? userId, string email)
    {
      try
      {
        if (userId != null)
        {
          return await _dbContext.Users.AnyAsync(u => u.Id != userId && u.Email == email);
        }

        return await _dbContext.Users.AnyAsync(u => u.Email == email);
      }
      catch (Exception)
      {
        return false;
      }
    }
    public async Task<bool> AtualizarImagemDePerfilAsync(UserModel user)
    {
      try
      {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }
    public async Task<bool> ExcluirUsuarioAsync(UserModel user)
    {
      try
      {
        await _dbContext.Users
        .Where(u => u.Id == user.Id)
        .ExecuteDeleteAsync();
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    // Listas de Usuário
    public async Task<UserListModel>? CriarListaAsync(UserListModel userList)
    {
      try
      {
        await _dbContext.UserLists.AddAsync(userList);
        await _dbContext.SaveChangesAsync();
        return userList;
      }
      catch (Exception)
      {
        return null;
      }
    }
    public async Task<UserListModel>? AtualizarListaAsync(UserListModel userList)
    {
      try
      {
        _dbContext.UserLists.Update(userList);
        await _dbContext.SaveChangesAsync();
        return userList;
      }
      catch (Exception)
      {
        return null;
      }
    }
    public async Task<UserListModel>? AdicionarJogoAListaAsync(UserListGameModel userGameList)
    {
      try
      {
        await _dbContext.UserListsItems.AddAsync(userGameList);
        await _dbContext.SaveChangesAsync();

        return await BuscarListaAsync(userGameList.ListId);
      }
      catch (Exception)
      {
        return null;
      }
    }
    public async Task<UserListModel>? BuscarListaAsync(int userListId)
    {
      try
      {
        return await _dbContext.UserLists.FindAsync(userListId);
      }
      catch (Exception)
      {
        return null;
      }
    }
    public async Task<List<UserListModel>> BuscarListasDeUsuarioAsync(int userId)
    {
      try
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
      catch (Exception)
      {
        return [];
      }
    }
    public async Task<UserListGameModel>? BuscarJogoDaListaAsync(int gameId, int listId)
    {
      return await _dbContext.UserListsItems
      .FirstOrDefaultAsync(uli => uli.GameId == gameId && uli.ListId == listId);
    }
    public async Task<bool> ExisteListaComMesmoNomeAsync(string name, int userId)
    {
      return await _dbContext.UserLists.AnyAsync(ul => ul.Name == name && ul.UserId == userId);
    }
    public async Task<bool> ExcluirListaAsync(UserListModel list)
    {
      _dbContext.UserLists.Remove(list);
      await _dbContext.SaveChangesAsync();
      return true;
    }
    public async Task<bool> RemoverJogoDaListaAsync(UserListGameModel gameList)
    {
      _dbContext.UserListsItems.Remove(gameList);
      await _dbContext.SaveChangesAsync();
      return true;
    }

    //Biblioteca de Usuário
    public async Task<bool> AdicionarJogoNaBibliotecaAsync(UserLibraryModel userLibraryGame)
    {
      try
      {
        await _dbContext.UserLibrary
        .AddAsync(userLibraryGame);
        await _dbContext.SaveChangesAsync();

        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }
    public async Task<List<UserLibraryModel>> BuscarJogosDaBibliotecaAsync(int id)
    {
      return await _dbContext.UserLibrary
              .Include(ul => ul.Game)
                      .ThenInclude(g => g.Publisher)
              .Include(ul => ul.User)
              .Where(ul => ul.UserId == id)
              .AsSplitQuery()
              .ToListAsync();
    }
    public async Task<UserLibraryModel>? BuscarPorIdJogoDaBibliotecaAsync(int userId, int gameId)
    {
      try
      {
        return await _dbContext.UserLibrary
        .FirstOrDefaultAsync(ul => ul.UserId == userId && ul.GameId == gameId);
      }
      catch (Exception)
      {
        return null;
      }
    }
    public async Task<bool> RemoverJogoDaBibliotecaAsync(UserLibraryModel library)
    {
      _dbContext.UserLibrary.Remove(library);
      await _dbContext.SaveChangesAsync();
      return true;
    }

    // Avaliações de Usuário
    public async Task<List<RatingModel>> AdicionarAvaliacaoAsync(RatingModel rating)
    {
      await _dbContext.Ratings.AddAsync(rating);
      await _dbContext.SaveChangesAsync();

      return await _dbContext.Ratings
        .Include(r => r.Game)
        .Include(r => r.User)
        .Where(r => r.UserId == rating.UserId)
        .AsSplitQuery()
        .ToListAsync();
    }
    public async Task<RatingModel> AtualizarAvaliacaoPorIdAsync(RatingModel rating)
    {
      _dbContext.Ratings.Update(rating);
      await _dbContext.SaveChangesAsync();
      return rating;
    }
    public async Task<List<RatingModel>> BuscarAvaliacoesDoUsuarioAsync(int userId)
    {
      return await _dbContext.Ratings
              .Include(gr => gr.Game)
              .Include(gr => gr.User)
              .Where(gr => gr.UserId == userId)
              .AsSplitQuery()
              .ToListAsync();
    }
    public async Task<RatingModel>? BuscarAvaliacaoPorUserEGameAsync(int userId, int gameId)
    {
      try
      {
        return await _dbContext.Ratings
        .Where(r => r.UserId == userId && r.GameId == gameId)
        .FirstOrDefaultAsync();
      }
      catch (Exception)
      {
        return null;
      }
    }
    public async Task<RatingModel>? BuscarAvaliacaoPorIdAsync(int ratingId)
    {
      return await _dbContext.Ratings
        .Include(r => r.Game)
        .Include(r => r.User)
        .Where(r => r.Id == ratingId)
        .AsSplitQuery()
        .FirstOrDefaultAsync();
    }
    public async Task<bool> ExcluirAvaliacaoAsync(RatingModel rating)
    {
      _dbContext.Ratings.Remove(rating);
      await _dbContext.SaveChangesAsync();
      return true;
    }
  }
}
