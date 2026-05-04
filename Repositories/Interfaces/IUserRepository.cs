using LudoVault.Model;

namespace LudoVault.Repositories.Interfaces
{
  public interface IUserRepository
  {
    // Usuário
    public Task<UserModel> CriarUsuarioAsync(UserModel user);
    public Task<UserModel> AtualizarUsuarioAsync(UserModel user, int id);
    public Task<UserModel> BuscarUsuarioPorIdAsync(int id);
    public Task<bool> VerificarEmailExistenteAsync(string email);

    // Listas de Usuário
    public Task<UserListModel> CriarUserListAsync(UserListModel userList);
    public Task<UserListModel> AtualizarUserListAsync(UserListModel userList, int userId, int listId);
    public Task<UserListModel> AdicionarJogoAListaAsync(UserListGameModel userGameList);
    public Task<List<UserListModel>> BuscarUserListsPorUsuarioAsync(int userId);
    public Task<UserListGameModel> BuscarItemDaListaAsync(int userListGameId);
    public Task<bool> ExisteListaComMesmoNomeAsync(string name, int userId);
    public Task<bool> ExisteUserListAsync(int listId, int userId);
    public Task<bool> JogoExisteNaListaAsync(int gameId, int listId);
    public Task<bool> DeletarUserListAsync(int listId);
    public Task<bool> RemoverJogoDaListaAsync(int listId, int gameId);

    // Biblioteca de Usuário
    public Task<bool> AdicionarJogoABibliotecaAsync(UserLibraryModel userListGame);
    public Task<List<UserLibraryModel>> BuscarJogosDaBiblioteca(int id);
    public Task<bool> RemoverJogoDaBiblioteca(int libraryId);
    public Task<bool> ExisteJogoNaBiblioteca(int userId, int gameId);

    // Avalaiações de Usuário
    public Task<List<GameRatingModel>> BuscarAvaliacoesDoUsuarioAsync(int userId);
  }
}
