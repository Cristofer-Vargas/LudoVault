using LudoVault.DTO.Requests;
using LudoVault.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LudoVault.Controllers
{
  [ApiController]
  [Route("[Controller]")]
  public class PublisherController(IPublisherServices publisherServices) : ControllerBase
  {
    private readonly IPublisherServices _publisherServices = publisherServices;

    [HttpPost]
    public async Task<IActionResult> CriarPublisher([FromBody] PublisherRequest publisher)
    {
      return Ok(await _publisherServices.CriarPublisherAsync(publisher));
    }

    [HttpGet]
    public async Task<IActionResult> BuscarTodos()
    {
      return Ok(await _publisherServices.BuscarTodasPublishersAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPublisher(int id)
    {
      return Ok(await _publisherServices.BuscarPublisherPorIdAsync(id));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarPublisher(int id, [FromBody] PublisherRequest publisher)
    {
      return Ok(await _publisherServices.AtualizarPublisherAsync(publisher, id));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> ExcluirPublisher(int id)
    {
      return Ok(await _publisherServices.ExcluirPublisherAsync(id));
    }
  }
}
