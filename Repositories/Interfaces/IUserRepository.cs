using LudoVault.Model;

namespace LudoVault.Repositories.Interfaces
{
  public interface IUserRepository
  {
    // Usuário
    public Task<UserModel>? CriarUsuarioAsync(UserModel user);
    public Task<UserModel>? AtualizarUsuarioAsync(UserModel user, int id);
    public Task<UserModel>? BuscarUsuarioPorIdAsync(int id);
    public Task<bool> VerificarEmailExistenteAsync(int? userId, string email);
    public Task<bool> AtualizarImagemDePerfilAsync(UserModel user);
    public Task<bool> ExcluirUsuarioAsync(UserModel user);

    // Listas de Usuário
    public Task<UserListModel>? CriarListaAsync(UserListModel userList);
    public Task<UserListModel>? AtualizarListaAsync(UserListModel userList, int userId, int listId);
    public Task<UserListModel>? AdicionarJogoAListaAsync(UserListGameModel userGameList);
    public Task<List<UserListModel>> BuscarListasDeUsuarioAsync(int userId);
    public Task<UserListModel>? BuscarListaAsync(int userListId);
    public Task<UserListGameModel>? BuscarJogoDaListaAsync(int userListGameId);
    public Task<bool> ExisteListaComMesmoNomeAsync(string name, int userId);
    public Task<bool> ExisteListaAsync(int listId, int userId);
    public Task<bool> ExisteJogoNaListaAsync(int gameId, int listId);
    public Task<bool> ExcluirListaAsync(int listId);
    public Task<bool> RemoverJogoDaListaAsync(int listId, int gameId); // trocar por id do jogo dentro de list

    // Biblioteca de Usuário
    public Task<bool> AdicionarJogoNaBibliotecaAsync(UserLibraryModel userListGame);
    public Task<List<UserLibraryModel>> BuscarJogosDaBibliotecaAsync(int id);
    public Task<bool> RemoverJogoDaBibliotecaAsync(int libraryId);
    public Task<bool> ExisteJogoNaBibliotecaAsync(int userId, int gameId);

    // Avalaiações de Usuário
    public Task<List<RatingModel>> AdicionarAvaliacaoAsync(RatingModel rating);
    public Task<List<RatingModel>> BuscarAvaliacoesDoUsuarioAsync(int userId);
    public Task<RatingModel> AtualizarAvaliacaoPorIdAsync(RatingModel rating, int ratingId);
    public Task<RatingModel>? BuscarAvaliacaoPorIdAsync(int ratingId);
    public Task<RatingModel>? BuscarAvaliacaoPorUserEGameAsync(int userId, int gameId);
    public Task<bool> ExcluirAvaliacaoAsync(RatingModel rating);
  }
}
