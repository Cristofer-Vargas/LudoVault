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
    public Task<Response<string>> DeletarUsuarioAsync(int userId);

    // Listas de Usuário
    public Task<Response<UserListListsResponse>> CriarUserListAsync(UserListRequest userList);
    public Task<Response<UserListListsResponse>> AtualizarUserListAsync(UserListRequest listRequest, int listId);
    public Task<Response<UserListListsResponse>> CriarGameInListAsync(UserListGameRequest userGameList, int userId);
    public Task<Response<UserListResponse>> BuscarUserListsAsync(int id);
    public Task<Response<string>> DeletarUserListAsync(int userId, int listId);
    public Task<Response<string>> DeletarGameInUserListAsync(int userId, int listId, int gameId);

    // Biblioteca de Usuário
    public Task<Response<string>> AdicionarJogoABibliotecaAsync(UserLibraryRequest userLibrary);
    public Task<Response<List<UserLibraryGameResponse>>> BuscarJogosDaBibliotecaAsync(int id);
    public Task<Response<string>> RemoverJogoDaBibliotecaAsync(int userId, int libraryId);

    // Avaliações de Usuários
    public Task<Response<UserRatingListGamesResponse>> AdicionarUserRatingAsync(UserRatingRequest userRating, int userId, int gameId);
    public Task<Response<UserRatingListGamesResponse>> BuscarUserRatingsAsync(int id);
    public Task<Response<UserRatingGameResponse>> BuscarRatingPorIdAsync(int ratingId);
    public Task<Response<UserRatingGameResponse>> AtualizarRatingAsync(UserRatingRequest userRating, int userId, int ratingId);
    public Task<Response<string>> RemoverRatingAsync(int userId, int ratingId);
  }
}
