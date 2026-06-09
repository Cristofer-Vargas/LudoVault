using LudoVault.Api.Controllers.Base;
using LudoVault.Application.DTO.Requests;
using LudoVault.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LudoVault.Api.Controllers
{
  [ApiController]
  [Route("[Controller]")]
  public class PublisherController(IPublisherServices publisherServices) : ControllerBase
  {
    private readonly IPublisherServices _publisherServices = publisherServices;

    [HttpPost]
    public async Task<IActionResult> CriarPublisher([FromBody] PublisherRequest publisher)
    {
      return this.GetResponse(await _publisherServices.CriarPublisherAsync(publisher));
    }

    [HttpGet]
    public async Task<IActionResult> BuscarTodos()
    {
      return this.GetResponse(await _publisherServices.BuscarTodasPublishersAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPublisher(int id)
    {
      return this.GetResponse(await _publisherServices.BuscarPublisherPorIdAsync(id));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarPublisher(int id, [FromBody] PublisherRequest publisher)
    {
      return this.GetResponse(await _publisherServices.AtualizarPublisherAsync(publisher, id));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> ExcluirPublisher(int id)
    {
      return this.GetResponse(await _publisherServices.ExcluirPublisherAsync(id));
    }
  }
}
