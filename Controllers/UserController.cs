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
      try
      {
        var u = await _userServices.AtualizarUsuarioAsync(user, id);
        return Ok(u);

      }
      catch (Exception e)
      {
        return BadRequest($"Erro: {e.Message}");
      }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BurcarUsuarioPorId(int id)
    {
      try
      {
        var user = await _userServices.BuscarUsuarioPorIdAsync(id);
        return Ok(user);

      }
      catch (Exception e)
      {
        return BadRequest($"Erro: {e.Message}");
      }
    }

    // Avaliaçãoes de Usuário
    [HttpGet("{id}/ratings")]
    public async Task<IActionResult> BuscarUserRatings(int id)
    {
      try
      {
        var userRatings = await _userServices.BuscarUserRatingsAsync(id);
        return Ok(userRatings);

      }
      catch (Exception e)
      {
        return BadRequest($"Erro: {e.Message}");
      }
    }

    // Listas de Usuários
    [HttpGet("{id}/lists")]
    public async Task<IActionResult> BuscarUserLists(int id)
    {
      try
      {
        var userLists = await _userServices.BuscarUserListsAsync(id);
        return Ok(userLists);

      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPost("create/list")]
    public async Task<IActionResult> CreateUserList([FromBody] UserListRequest userList)
    {
      try
      {
        var userLists = await _userServices.CriarUserListAsync(userList);
        return Ok(userLists);

      }
      catch (Exception e)
      {

        return BadRequest(e.Message);
      }
    }

    [HttpPost("{userId}/list/game")]
    public async Task<IActionResult> AddGameInUserList([FromBody] UserListGameRequest game, int userId)
    {
      try
      {
        var atualListWithGame = await _userServices.CriarGameInListAsync(game, userId);
        return Ok(atualListWithGame);

      }
      catch (Exception e)
      {

        return BadRequest(e.Message);
      }
    }

    [HttpPut("update/list/{listId}")]
    public async Task<IActionResult> AtualizarUserList([FromBody] UserListRequest userList, int listId)
    {
      try
      {
        var userListUp = await _userServices.AtualizarUserListAsync(userList, listId);
        return Ok(userListUp);

      }
      catch (Exception e)
      {

        return BadRequest(e.Message);
      }
    }

    [HttpDelete("{userId}/list/{listId}/game/{gameId}")]
    public async Task<IActionResult> DeletarGameDeUserList(int userId, int listId, int gameId)
    {
      try
      {
        if (await _userServices.DeletarGameInUserListAsync(userId, listId, gameId))
        {
          return Ok("Deletado com sucesso!");
        }

        throw new Exception("Não foi possivel deletar esse game!");
      }
      catch (Exception e)
      {

        return BadRequest(e.Message);
      }
    }

    [HttpDelete("{userId}/list/{listId}")]
    public async Task<IActionResult> DeletarUserList(int userId, int listId)
    {
      try
      {
        if (await _userServices.DeletarUserListAsync(userId, listId))
        {
          return Ok("Deletado com sucesso!");
        }

        throw new Exception("Não foi possivel deletar esse game!");
      }
      catch (Exception e)
      {

        return BadRequest(e.Message);
      }
    }

    // Biblioteca de Usuário
    [HttpGet("{userId}/library")]
    public async Task<IActionResult> BuscarBibliotecadeUser(int userId)
    {
      try
      {
        var userLibrary = await _userServices.BuscarJogosDaBiblioteca(userId);
        return Ok(userLibrary);
      }
      catch (Exception e)
      {

        return BadRequest(e.Message);
      }
    }

    [HttpPost("library")]
    public async Task<IActionResult> AdicionarJogoABiblioteca([FromBody] UserLibraryRequest userLibrary)
    {
      try
      {
        await _userServices.AdicionarJogoABibliotecaAsync(userLibrary);
        return Ok("Adicionado com Sucesso!");

      }
      catch (Exception e)
      {

        return BadRequest(e.Message);
      }
    }

    [HttpDelete("{userId}/library/{libraryId}")]
    public async Task<IActionResult> RemoverJogoDaBiblioteca(int userId, int libraryId)
    {
      try
      {
        await _userServices.BuscarUsuarioPorIdAsync(userId);
        await _userServices.RemoverJogoDaBiblioteca(libraryId);

        return Ok("Removido com sucesso!");
      }
      catch (Exception e)
      {

        return BadRequest(e.Message);
      }
    }
  }
}
