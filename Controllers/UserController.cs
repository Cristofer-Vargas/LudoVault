using LudoVault.DTO.Requests;
using LudoVault.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LudoVault.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class UserController(IUserServices userServices) : ControllerBase
  {
    private readonly IUserServices _userServices = userServices;

    // Usuário
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarUsuario([FromBody] UserRequest user, int id)
    {
      return Ok(await _userServices.AtualizarUsuarioAsync(user, id));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BurcarUsuarioPorId(int id)
    {
      return Ok(await _userServices.BuscarUsuarioPorIdAsync(id));
    }

    [HttpDelete("{userId}/remove/profile/image")]
    public async Task<IActionResult> RemoverImagemDePerfil(int userId)
    {
      return Ok(await _userServices.RemoverImagemDePerfilAsync(userId));
    }

    [HttpDelete("{userId}/delete")]
    public async Task<IActionResult> DeletarUsuario(int userId)
    {
      return Ok(await _userServices.DeletarUsuarioAsync(userId));
    }

    // Avaliaçãoes de Usuário
    [HttpGet("{id}/ratings")]
    public async Task<IActionResult> BuscarUserRatings(int id)
    {
      return Ok(await _userServices.BuscarUserRatingsAsync(id));
    }

    [HttpPost("{userId}/rating/game/{gameId}")]
    public async Task<IActionResult> AdicionarUserRating([FromBody] UserRatingRequest userRatingRequest, [FromRoute] int userId, [FromRoute] int gameId)
    {
      return Ok(await _userServices.AdicionarUserRatingAsync(userRatingRequest, userId, gameId));
    }

    [HttpDelete("{userId}/rating/{ratingId}")]
    public async Task<IActionResult> RemoverUserRating(int userId, int ratingId)
    {
      return Ok(await _userServices.RemoverRatingAsync(userId, ratingId));
    }

    [HttpPut("{userId}/rating/{ratingId}")]
    public async Task<IActionResult> AtualizarUserRating([FromBody] UserRatingRequest userRatingRequest, [FromRoute] int userId, [FromRoute] int ratingId)
    {
      return Ok(await _userServices.AtualizarRatingAsync(userRatingRequest, userId, ratingId));
    }

    // Listas de Usuários
    [HttpGet("{id}/lists")]
    public async Task<IActionResult> BuscarUserLists(int id)
    {
      return Ok(await _userServices.BuscarUserListsAsync(id));
    }

    [HttpPost("create/list")]
    public async Task<IActionResult> CreateUserList([FromBody] UserListRequest userList)
    {
      return Ok(await _userServices.CriarUserListAsync(userList));
    }

    [HttpPost("{userId}/list/game")]
    public async Task<IActionResult> AddGameInUserList([FromBody] UserListGameRequest game, int userId)
    {
      return Ok(await _userServices.CriarGameInListAsync(game, userId));
    }

    [HttpPut("update/list/{listId}")]
    public async Task<IActionResult> AtualizarUserList([FromBody] UserListRequest userList, int listId)
    {
      return Ok(await _userServices.AtualizarUserListAsync(userList, listId));
    }

    [HttpDelete("{userId}/list/{listId}/game/{gameId}")]
    public async Task<IActionResult> DeletarGameDeUserList(int userId, int listId, int gameId)
    {
      return Ok(await _userServices.DeletarGameInUserListAsync(userId, listId, gameId));
    }

    [HttpDelete("{userId}/list/{listId}")]
    public async Task<IActionResult> DeletarUserList(int userId, int listId)
    {
      return Ok(await _userServices.DeletarUserListAsync(userId, listId));
    }

    // Biblioteca de Usuário
    [HttpGet("{userId}/library")]
    public async Task<IActionResult> BuscarBibliotecadeUser(int userId)
    {
      return Ok(await _userServices.BuscarJogosDaBibliotecaAsync(userId));
    }

    [HttpPost("library")]
    public async Task<IActionResult> AdicionarJogoABiblioteca([FromBody] UserLibraryRequest userLibrary)
    {
      return Ok(await _userServices.AdicionarJogoABibliotecaAsync(userLibrary));
    }

    [HttpDelete("{userId}/library/{libraryId}")]
    public async Task<IActionResult> RemoverJogoDaBiblioteca(int userId, int libraryId)
    {
      return Ok(await _userServices.RemoverJogoDaBibliotecaAsync(userId, libraryId));
    }
  }
}
