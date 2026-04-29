﻿using LudoVault.Services.Interfaces;
using LudoVault.Services.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LudoVault.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameServices _gameServices;

        public GameController(IGameServices gameServices)
        {
            _gameServices = gameServices;
        }

        [HttpGet("{id}/ratings")]
        public async Task<IActionResult> BuscarRatingsDeGame(int id)
        {
             try
             {
                var gameRatings = await _gameServices.BuscarRatingsPorIdGame(id);
                return Ok(gameRatings);

             } catch (Exception e)
             {
                return NotFound(e.Message);
             }
        }

        [HttpGet]
        public async Task<IActionResult> BuscarGames()
        {
            try
            {
                var games = await _gameServices.BuscarGames();
                return Ok(games);

            } catch(Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarGamePorId(int id)
        {
            try
            {
                var game = await _gameServices.BuscarGamePorId(id);
                return Ok(game);

            } catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CriarGame([FromBody] GameRequest game)
        {
            try
            {
                var newGame = await _gameServices.CriarGame(game);
                return Ok(newGame);

            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarGame([FromBody] GameRequest game, int id)
        {
            try
            {
                var gameUp = await _gameServices.AtualizarGame(game, id);
                return Ok(gameUp);

            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarGame(int id)
        {
            try
            {
                await _gameServices.RemoverGame(id);
                return Ok("Excluido com sucesso!");

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
