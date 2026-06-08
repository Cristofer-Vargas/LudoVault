using LudoVault.Application.DTO.Requests;
using LudoVault.Application.DTO.Responses;
using LudoVault.Application.Validations.Base;

namespace LudoVault.Application.Interfaces.Services
{
  public interface IUserServices
  {
    // Usuário
    public Task<Response<UserResponse>> CriarUsuarioAsync(UserRequest user);
    public Task<Response<UserResponse>> AtualizarUsuarioAsync(UserRequest user, int userId);
    public Task<Response<UserResponse>> AtualizarSenhaUsuarioAsync(UserPasswordUpdateRequest request, int userId);
    public Task<Response<UserResponse>> BuscarUsuarioPorIdAsync(int id);
    public Task<Response<UserResponse>> AdicionarImagemDePerfilAsync(IFormFile image, int userId);
    public Task<Response<UserResponse>> RemoverImagemDePerfilAsync(int userId);
    public Task<Response<string>> ExcluirUsuarioAsync(int userId);

    // Listas de Usuário
    public Task<Response<UserListListsResponse>> CriarListaAsync(UserListRequest userList, int userId);
    public Task<Response<UserListListsResponse>> AtualizarListaAsync(UserListRequest userList, int userId, int listId);
    public Task<Response<UserListListsResponse>> AdicionarJogoAListaAsync(int listId, int gameId, int userId);
    public Task<Response<UserListResponse>> BuscarListasDeUsuarioAsync(int id);
    public Task<Response<string>> ExcluirListaAsync(int userId, int listId);
    public Task<Response<UserListListsResponse>> RemoverJogoDeListaAsync(int userId, int listId, int gameId);

    // Biblioteca de Usuário
    public Task<Response<List<UserLibraryGameResponse>>> AdicionarJogoABibliotecaAsync(int userId, int gameId);
    public Task<Response<List<UserLibraryGameResponse>>> BuscarJogosDaBibliotecaAsync(int id);
    public Task<Response<List<UserLibraryGameResponse>>> RemoverJogoDaBibliotecaAsync(int userId, int gameId);

    // Avaliações de Usuários
    public Task<Response<UserRatingListGamesResponse>> AdicionarAvaliacaoAsync(UserRatingRequest userRating, int userId, int gameId);
    public Task<Response<UserRatingGameResponse>> AtualizarAvaliacaoAsync(UserRatingRequest userRating, int userId, int ratingId);
    public Task<Response<UserRatingListGamesResponse>> BuscarAvaliacoesAsync(int id);
    public Task<Response<string>> ExcluirAvaliacaoAsync(int userId, int ratingId);
  }
}
