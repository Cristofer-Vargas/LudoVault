using LudoVault.Data;
using LudoVault.Model;
using LudoVault.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LudoVault.Repositories
{
  public class UserRepository(MysqlContext dbContext, ILogger<UserRepository> logger) : IUserRepository
  {
    private readonly MysqlContext _dbContext = dbContext;
    private readonly ILogger<UserRepository> _logger = logger;

    // Usuário
    public async Task<UserModel>? CriarUsuarioAsync(UserModel user)
    {
      using var transaction = await _dbContext.Database.BeginTransactionAsync();

      try
      {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
        return user;
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao criar usuário {UNAME} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", user.Name, e.Message, e.Source);
        await _dbContext.Database.RollbackTransactionAsync();
        return null;
      }
    }
    public async Task<UserModel>? AtualizarUsuarioAsync(UserModel user)
    {
      using var transaction = await _dbContext.Database.BeginTransactionAsync();

      try
      {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
        return user;
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao atualizar usuário {UID}:{UNAME} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", user.Id, user.Name, e.Message, e.Source);
        await _dbContext.Database.RollbackTransactionAsync();
        return null;
      }
    }
    public async Task<UserModel>? BuscarUsuarioPorIdAsync(int id)
    {
      try
      {
        return await _dbContext.Users.FindAsync(id);
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao buscar usuário {UID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", id, e.Message, e.Source);
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
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao verificar email {EMAIL} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", email, e.Message, e.Source);
        await _dbContext.Database.RollbackTransactionAsync();
        return false;
      }
    }
    public async Task<bool> AtualizarImagemDePerfilAsync(UserModel user)
    {
      using var transaction = await _dbContext.Database.BeginTransactionAsync();

      try
      {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
        return true;
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao atualizar usuário {UID}:{UNAME} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", user.Id, user.Name, e.Message, e.Source);
        await _dbContext.Database.RollbackTransactionAsync();
        return false;
      }
    }
    public async Task<bool> ExcluirUsuarioAsync(UserModel user)
    {
      using var transaction = await _dbContext.Database.BeginTransactionAsync();

      try
      {
        await _dbContext.Users
        .Where(u => u.Id == user.Id)
        .ExecuteDeleteAsync();
        await _dbContext.Database.CommitTransactionAsync();
        return true;
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao excluir usuário {UID}:{UNAME} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", user.Id, user.Name, e.Message, e.Source);
        await _dbContext.Database.RollbackTransactionAsync();
        return false;
      }
    }

    // Listas de Usuário
    public async Task<UserListModel>? CriarListaAsync(UserListModel list)
    {
      using var transaction = await _dbContext.Database.BeginTransactionAsync();

      try
      {
        await _dbContext.UserLists.AddAsync(list);
        await _dbContext.SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
        return list;
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao criar lista {LNAME} de usuário {UID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", list.Name, list.UserId, e.Message, e.Source);
        await _dbContext.Database.RollbackTransactionAsync();
        return null;
      }
    }
    public async Task<UserListModel>? AtualizarListaAsync(UserListModel list)
    {
      using var transaction = await _dbContext.Database.BeginTransactionAsync();

      try
      {
        _dbContext.UserLists.Update(list);
        await _dbContext.SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
        return list;
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao atualizar lista {LID}:{LNAME} de usuário {UID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", list.Id, list.Name, list.UserId, e.Message, e.Source);
        await _dbContext.Database.RollbackTransactionAsync();
        return null;
      }
    }
    public async Task<UserListModel>? AdicionarJogoAListaAsync(UserListGameModel listGame)
    {
      using var transaction = await _dbContext.Database.BeginTransactionAsync();

      try
      {
        await _dbContext.UserListsItems.AddAsync(listGame);
        await _dbContext.SaveChangesAsync();
        var listWithGame = await BuscarListaAsync(listGame.ListId);
        await _dbContext.Database.CommitTransactionAsync();
        return listWithGame;
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao adicionar jogo {GID} a lista {LID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}",
          listGame.GameId, listGame.ListId, e.Message, e.Source);
        await _dbContext.Database.RollbackTransactionAsync();
        return null;
      }
    }
    public async Task<UserListModel>? BuscarListaAsync(int listId)
    {
      try
      {
        return await _dbContext.UserLists.FindAsync(listId);
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao buscar lista {LID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", listId, e.Message, e.Source);
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
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao buscar todas as listas de {UID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", userId, e.Message, e.Source);
        return [];
      }
    }
    public async Task<UserListGameModel>? BuscarJogoDaListaAsync(int gameId, int listId)
    {
      try
      {
        return await _dbContext.UserListsItems.FirstOrDefaultAsync(uli => uli.GameId == gameId && uli.ListId == listId);
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao buscar jogo {GID} da lista {LID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", gameId, listId, e.Message, e.Source);
        return null;
      }
    }
    public async Task<bool> ExisteListaComMesmoNomeAsync(string name, int userId)
    {
      try
      {
        return await _dbContext.UserLists.AnyAsync(ul => ul.Name == name && ul.UserId == userId);
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao conferir nome {LNAME} de listas do usuário {UID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", name, userId, e.Message, e.Source);
        return false;
      }
    }
    public async Task<bool> ExcluirListaAsync(UserListModel list)
    {
      using var transaction = await _dbContext.Database.BeginTransactionAsync();

      try
      {
        _dbContext.UserLists.Remove(list);
        await _dbContext.SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
        return true;
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao excluir lista {LID} de usuário {UID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", list.Id, list.UserId, e.Message, e.Source);
        await _dbContext.Database.RollbackTransactionAsync();
        return false;
      }
    }
    public async Task<bool> RemoverJogoDaListaAsync(UserListGameModel listGame)
    {
      using var transaction = await _dbContext.Database.BeginTransactionAsync();

      try
      {
        _dbContext.UserListsItems.Remove(listGame);
        await _dbContext.SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
        return true;
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao remover jogo {GID} da lista {LID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}",
          listGame.GameId, listGame.ListId, e.Message, e.Source);
        await _dbContext.Database.RollbackTransactionAsync();
        return false;
      }
    }

    //Biblioteca de Usuário
    public async Task<bool> AdicionarJogoNaBibliotecaAsync(UserLibraryModel libraryGame)
    {
      using var transaction = await _dbContext.Database.BeginTransactionAsync();

      try
      {
        await _dbContext.UserLibrary
        .AddAsync(libraryGame);
        await _dbContext.SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
        return true;
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao adicionar jogo {GID} a biblioteca de {UID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}",
          libraryGame.GameId, libraryGame.UserId, e.Message, e.Source);
        await _dbContext.Database.RollbackTransactionAsync();
        return false;
      }
    }
    public async Task<List<UserLibraryModel>> BuscarJogosDaBibliotecaAsync(int id)
    {
      try
      {
        return await _dbContext.UserLibrary
              .Include(ul => ul.Game)
                      .ThenInclude(g => g.Publisher)
              .Include(ul => ul.User)
              .Where(ul => ul.UserId == id)
              .AsSplitQuery()
              .ToListAsync();

      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao buscar jogos da biblioteca do usuário {UID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", id, e.Message, e.Source);
        return [];
      }
    }
    public async Task<UserLibraryModel>? BuscarPorIdJogoDaBibliotecaAsync(int userId, int gameId)
    {
      try
      {
        return await _dbContext.UserLibrary
        .FirstOrDefaultAsync(ul => ul.UserId == userId && ul.GameId == gameId);
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao buscar jogo {GID} da biblioteca do usuário {UID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", gameId, userId, e.Message, e.Source);
        return null;
      }
    }
    public async Task<bool> RemoverJogoDaBibliotecaAsync(UserLibraryModel library)
    {
      using var transaction = await _dbContext.Database.BeginTransactionAsync();

      try
      {
        _dbContext.UserLibrary.Remove(library);
        await _dbContext.SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
        return true;
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao remover jogo {GID} da biblioteca do usuário {UID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}",
          library.GameId, library.UserId, e.Message, e.Source);
        await _dbContext.Database.RollbackTransactionAsync();
        return false;
      }
    }

    // Avaliações de Usuário
    public async Task<List<RatingModel>> AdicionarAvaliacaoAsync(RatingModel rating)
    {
      using var transaction = await _dbContext.Database.BeginTransactionAsync();

      try
      {
        await _dbContext.Ratings.AddAsync(rating);
        await _dbContext.SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
        return await BuscarAvaliacoesDoUsuarioAsync(rating.UserId);
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao adicionar avaliação ao jogo {GID} por usuário {UID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}",
          rating.GameId, rating.UserId, e.Message, e.Source);
        await _dbContext.Database.RollbackTransactionAsync();
        return [];
      }
    }
    public async Task<RatingModel>? AtualizarAvaliacaoPorIdAsync(RatingModel rating)
    {
      using var transaction = await _dbContext.Database.BeginTransactionAsync();

      try
      {
        _dbContext.Ratings.Update(rating);
        await _dbContext.SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
        return rating;
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao atualizar avaliação do jogo {GID} do usuário {UID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}",
          rating.GameId, rating.UserId, e.Message, e.Source);
        await _dbContext.Database.RollbackTransactionAsync();
        return null;
      }
    }
    public async Task<List<RatingModel>> BuscarAvaliacoesDoUsuarioAsync(int userId)
    {
      try
      {
        return await _dbContext.Ratings
              .Include(gr => gr.Game)
              .Include(gr => gr.User)
              .Where(gr => gr.UserId == userId)
              .AsSplitQuery()
              .ToListAsync();
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao buscar avaliações do usuário {UID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", userId, e.Message, e.Source);
        return [];
      }
    }
    public async Task<RatingModel>? BuscarAvaliacaoPorUserEGameAsync(int userId, int gameId)
    {
      try
      {
        return await _dbContext.Ratings
        .Where(r => r.UserId == userId && r.GameId == gameId)
        .FirstOrDefaultAsync();
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao buscar avaliações do usuário {UID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", userId, e.Message, e.Source);
        return null;
      }
    }
    public async Task<RatingModel>? BuscarAvaliacaoPorIdAsync(int ratingId)
    {
      try
      {
        return await _dbContext.Ratings
        .Include(r => r.Game)
        .Include(r => r.User)
        .Where(r => r.Id == ratingId)
        .AsSplitQuery()
        .FirstOrDefaultAsync();
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao buscar avaliação {RID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", ratingId, e.Message, e.Source);
        return null;
      }
    }
    public async Task<bool> ExcluirAvaliacaoAsync(RatingModel rating)
    {
      using var transaction = await _dbContext.Database.BeginTransactionAsync();

      try
      {
        _dbContext.Ratings.Remove(rating);
        await _dbContext.SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
        return true;
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao excluir avaliação {RID} do usuário {UID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}",
          rating.Id, rating.UserId, e.Message, e.Source);
        await _dbContext.Database.RollbackTransactionAsync();
        return false;
      }
    }
  }
}
