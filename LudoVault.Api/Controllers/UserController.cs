using LudoVault.Application.DTO.Requests;
using LudoVault.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LudoVault.Api.Controllers
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

    [HttpPut("{id}/update-password")]
    public async Task<IActionResult> AtualizarSenhaUsuario([FromBody] UserPasswordUpdateRequest request, int id)
    {
      return Ok(await _userServices.AtualizarSenhaUsuarioAsync(request, id));
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
      return Ok(await _userServices.ExcluirUsuarioAsync(userId));
    }

    // Avaliaçãoes de Usuário
    [HttpGet("{id}/ratings")]
    public async Task<IActionResult> BuscarUserRatings(int id)
    {
      return Ok(await _userServices.BuscarAvaliacoesAsync(id));
    }

    [HttpPost("{userId}/rating/game/{gameId}")]
    public async Task<IActionResult> AdicionarUserRating([FromBody] UserRatingRequest userRatingRequest, [FromRoute] int userId, [FromRoute] int gameId)
    {
      return Ok(await _userServices.AdicionarAvaliacaoAsync(userRatingRequest, userId, gameId));
    }

    [HttpDelete("{userId}/rating/{ratingId}")]
    public async Task<IActionResult> RemoverUserRating(int userId, int ratingId)
    {
      return Ok(await _userServices.ExcluirAvaliacaoAsync(userId, ratingId));
    }

    [HttpPut("{userId}/rating/{ratingId}")]
    public async Task<IActionResult> AtualizarUserRating([FromBody] UserRatingRequest userRatingRequest, [FromRoute] int userId, [FromRoute] int ratingId)
    {
      return Ok(await _userServices.AtualizarAvaliacaoAsync(userRatingRequest, userId, ratingId));
    }

    // Listas de Usuários
    [HttpGet("{id}/lists")]
    public async Task<IActionResult> BuscarUserLists(int id)
    {
      return Ok(await _userServices.BuscarListasDeUsuarioAsync(id));
    }

    [HttpPost("{userId}/create/list")]
    public async Task<IActionResult> CreateUserList([FromBody] UserListRequest userList, int userId)
    {
      return Ok(await _userServices.CriarListaAsync(userList, userId));
    }

    [HttpPost("{userId}/list/{listId}/game/{gameId}")]
    public async Task<IActionResult> AddGameInUserList(int listId, int gameId, int userId)
    {
      return Ok(await _userServices.AdicionarJogoAListaAsync(listId, gameId, userId));
    }

    [HttpPut("{userId}/update/list/{listId}")]
    public async Task<IActionResult> AtualizarUserList([FromBody] UserListRequest userList, int userId, int listId)
    {
      return Ok(await _userServices.AtualizarListaAsync(userList, userId, listId));
    }

    [HttpDelete("{userId}/list/{listId}/game/{gameId}")]
    public async Task<IActionResult> DeletarGameDeUserList(int userId, int listId, int gameId)
    {
      return Ok(await _userServices.RemoverJogoDeListaAsync(userId, listId, gameId));
    }

    [HttpDelete("{userId}/list/{listId}")]
    public async Task<IActionResult> DeletarUserList(int userId, int listId)
    {
      return Ok(await _userServices.ExcluirListaAsync(userId, listId));
    }

    // Biblioteca de Usuário
    [HttpGet("{userId}/library")]
    public async Task<IActionResult> BuscarBibliotecadeUser(int userId)
    {
      return Ok(await _userServices.BuscarJogosDaBibliotecaAsync(userId));
    }

    [HttpPost("{userId}/library/game/{gameId}")]
    public async Task<IActionResult> AdicionarJogoABiblioteca(int userId, int gameId)
    {
      return Ok(await _userServices.AdicionarJogoABibliotecaAsync(userId, gameId));
    }

    [HttpDelete("{userId}/library/game/{gameId}")]
    public async Task<IActionResult> RemoverJogoDaBiblioteca(int userId, int gameId)
    {
      return Ok(await _userServices.RemoverJogoDaBibliotecaAsync(userId, gameId));
    }
  }
}
