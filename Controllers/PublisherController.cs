using LudoVault.Services.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LudoVault.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class PublisherController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> BuscarTodos()
        {
            return Ok("Hello World!");
        }
    }
}
