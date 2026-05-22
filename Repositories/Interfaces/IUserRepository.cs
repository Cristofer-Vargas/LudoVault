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
    public Task<bool> DeletarUsuarioAsync(UserModel user);

    // Listas de Usuário
    public Task<UserListModel>? CriarUserListAsync(UserListModel userList);
    public Task<UserListModel>? AtualizarUserListAsync(UserListModel userList, int userId, int listId);
    public Task<UserListModel>? AdicionarJogoAListaAsync(UserListGameModel userGameList);
    public Task<List<UserListModel>> BuscarUserListsPorUsuarioAsync(int userId);
    public Task<UserListModel>? BuscarListaAsync(int userListId);
    public Task<UserListGameModel>? BuscarItemDaListaAsync(int userListGameId);
    public Task<bool> ExisteListaComMesmoNomeAsync(string name, int userId);
    public Task<bool> ExisteUserListAsync(int listId, int userId);
    public Task<bool> JogoExisteNaListaAsync(int gameId, int listId);
    public Task<bool> DeletarUserListAsync(int listId);
    public Task<bool> RemoverJogoDaListaAsync(int listId, int gameId); // trocar por id do jogo dentro de list

    // Biblioteca de Usuário
    public Task<bool> AdicionarJogoABibliotecaAsync(UserLibraryModel userListGame);
    public Task<List<UserLibraryModel>> BuscarJogosDaBibliotecaAsync(int id);
    public Task<bool> RemoverJogoDaBibliotecaAsync(int libraryId);
    public Task<bool> ExisteJogoNaBibliotecaAsync(int userId, int gameId);

    // Avalaiações de Usuário
    public Task<List<RatingModel>> AdicionarUserRatingAsync(RatingModel rating);
    public Task<List<RatingModel>> BuscarAvaliacoesDoUsuarioAsync(int userId);
    public Task<RatingModel> AtualizarRatingPorIdAsync(RatingModel rating, int ratingId);
    public Task<RatingModel>? BuscarRatingPorIdAsync(int ratingId);
    public Task<RatingModel>? BuscarRatingPorUserEGameAsync(int userId, int gameId);
    public Task<bool> RemoverRatingAsync(RatingModel rating);
  }
}
