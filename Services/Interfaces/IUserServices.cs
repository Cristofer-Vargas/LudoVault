using LudoVault.DTO.Requests;
using LudoVault.DTO.Responses;
using LudoVault.Services.Validations.Base;

namespace LudoVault.Services.Interfaces
{
  public interface IUserServices
  {
    // Usuário
    public Task<Response<UserResponse>> CriarUsuarioAsync(UserRequest user);
    public Task<Response<UserResponse>> AtualizarUsuarioAsync(UserRequest user, int id);
    public Task<Response<UserResponse>> BuscarUsuarioPorIdAsync(int id);
    public Task<Response<string>> AdicionarImagemDePerfilAsync(IFormFile image, int userId);
    public Task<Response<string>> RemoverImagemDePerfilAsync(int userId);
    public Task<Response<string>> ExcluirUsuarioAsync(int userId);

    // Listas de Usuário
    public Task<Response<UserListListsResponse>> CriarListaAsync(UserListRequest userList);
    public Task<Response<UserListListsResponse>> AtualizarListaAsync(UserListRequest listRequest, int listId);
    public Task<Response<UserListListsResponse>> AdicionarJogoAListaAsync(UserListGameRequest userGameList, int userId);
    public Task<Response<UserListResponse>> BuscarListasDeUsuarioAsync(int id);
    public Task<Response<string>> ExcluirListaAsync(int userId, int listId);
    public Task<Response<string>> RemoverJogoDeListaAsync(int userId, int listId, int gameId);

    // Biblioteca de Usuário
    public Task<Response<string>> AdicionarJogoABibliotecaAsync(UserLibraryRequest userLibrary);
    public Task<Response<List<UserLibraryGameResponse>>> BuscarJogosDaBibliotecaAsync(int id);
    public Task<Response<string>> RemoverJogoDaBibliotecaAsync(int userId, int libraryId);

    // Avaliações de Usuários
    public Task<Response<UserRatingListGamesResponse>> AdicionarAvaliacaoAsync(UserRatingRequest userRating, int userId, int gameId);
    public Task<Response<UserRatingListGamesResponse>> BuscarAvaliacoesAsync(int id);
    public Task<Response<UserRatingGameResponse>> BuscarAvaliacaoPorIdAsync(int ratingId);
    public Task<Response<UserRatingGameResponse>> AtualizarAvaliacaoAsync(UserRatingRequest userRating, int userId, int ratingId);
    public Task<Response<string>> ExcluirAvaliacaoAsync(int userId, int ratingId);
  }
}
