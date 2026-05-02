﻿using LudoVault.Services;
using LudoVault.Services.Interfaces;
using LudoVault.Services.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LudoVault.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class PublisherController(IPublisherServices publisherServices) : ControllerBase
    {
        private readonly IPublisherServices _publisherServices = publisherServices;

        // Desenvolvedora
        [HttpPost]
        public async Task<IActionResult> CriarPublisher([FromBody] PublisherRequest publisher)
        {
            try
            {
                var p = await _publisherServices.CriarPublisherAsync(publisher);
                return Ok(p);

            } catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> BuscarTodos()
        {
            try 
            {
                var publisherList = await _publisherServices.BuscarTodasPublishersAsync();
                return Ok(publisherList);

            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPublisher(int id)
        {
            try
            {
                var publisher = await _publisherServices.BuscarPublisherPorIdAsync(id);
                return Ok(publisher);

            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarPublisher(int id, [FromBody] PublisherRequest publisher)
        {
            try
            {
                var updatedPublisher = await _publisherServices.AtualizarPublisherAsync(publisher, id);
                return Ok(updatedPublisher);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirPublisher(int id)
        {
            try
            {
                bool excluded = await _publisherServices.ExcluirPublisherAsync(id);
                return Ok("Publisher Excluido com sucesso");

            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
