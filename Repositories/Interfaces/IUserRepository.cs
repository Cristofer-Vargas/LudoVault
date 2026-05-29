using LudoVault.Model;

namespace LudoVault.Repositories.Interfaces
{
  public interface IUserRepository
  {
    // Usuário
    public Task<UserModel>? CriarUsuarioAsync(UserModel user);
    public Task<UserModel>? AtualizarUsuarioAsync(UserModel user);
    public Task<UserModel>? BuscarUsuarioPorIdAsync(int id);
    public Task<bool> VerificarEmailExistenteAsync(int? userId, string email);
    public Task<bool> AtualizarImagemDePerfilAsync(UserModel user);
    public Task<bool> ExcluirUsuarioAsync(UserModel user);

    // Listas de Usuário
    public Task<UserListModel>? CriarListaAsync(UserListModel userList);
    public Task<UserListModel>? AtualizarListaAsync(UserListModel userList);
    public Task<UserListModel>? AdicionarJogoAListaAsync(UserListGameModel userGameList);
    public Task<List<UserListModel>> BuscarListasDeUsuarioAsync(int userId);
    public Task<UserListModel>? BuscarListaAsync(int userListId);
    public Task<UserListGameModel>? BuscarJogoDaListaAsync(int gameId, int listId);
    public Task<bool> ExisteListaComMesmoNomeAsync(string name, int userId);
    public Task<bool> ExcluirListaAsync(UserListModel list);
    public Task<bool> RemoverJogoDaListaAsync(UserListGameModel game); // trocar por id do jogo dentro de list

    // Biblioteca de Usuário
    public Task<bool> AdicionarJogoNaBibliotecaAsync(UserLibraryModel userListGame);
    public Task<List<UserLibraryModel>> BuscarJogosDaBibliotecaAsync(int id);
    public Task<UserLibraryModel>? BuscarPorIdJogoDaBibliotecaAsync(int userId, int gameId);
    public Task<bool> RemoverJogoDaBibliotecaAsync(UserLibraryModel gameLibrary);

    // Avalaiações de Usuário
    public Task<List<RatingModel>> AdicionarAvaliacaoAsync(RatingModel rating);
    public Task<RatingModel>? AtualizarAvaliacaoPorIdAsync(RatingModel rating);
    public Task<List<RatingModel>> BuscarAvaliacoesDoUsuarioAsync(int userId);
    public Task<RatingModel>? BuscarAvaliacaoPorIdAsync(int ratingId);
    public Task<RatingModel>? BuscarAvaliacaoPorUserEGameAsync(int userId, int gameId);
    public Task<bool> ExcluirAvaliacaoAsync(RatingModel rating);
  }
}
