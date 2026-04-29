﻿using LudoVault.Data;
using LudoVault.Model;
using LudoVault.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LudoVault.Repositories
{
    public class GameRepository : IGameRepository
    {
        public readonly MysqlContext _dbContext;
        public GameRepository(MysqlContext dbContext)
        {
            _dbContext = dbContext;
        }

       public async Task<GameModel> Criar(GameModel game)
        {
            // Salva o Game para gerar o ID
            await _dbContext.AddAsync(game);
            await _dbContext.SaveChangesAsync();

            foreach (var platform in game.GamePlatforms) { platform.GameId = game.Id; } // Para cada plataforma, e genero
            foreach (var genre in game.GameGenres) { genre.GameId = game.Id; }          // adiciona o ID do game para preencher no banco

            // Adiciona explicitamente ao contexto
            if (game.GamePlatforms.Count > 0) { _dbContext.GamePlatforms.UpdateRange(game.GamePlatforms); }
            if (game.GameGenres.Count > 0) { _dbContext.GameGenres.UpdateRange(game.GameGenres); }

            await _dbContext.SaveChangesAsync();

            return await BuscarPorId(game.Id) ?? throw new Exception("Erro ao recuperar o Jogo recém criado!");
        }

        public async Task<GameModel> Atualizar(GameModel game, int id)
        {
            var currentGame = await BuscarPorId(id);

            currentGame.Name = game.Name;
            currentGame.Description = game.Description;
            currentGame.ImageUrl = game.ImageUrl;
            currentGame.PublisherId = game.PublisherId;
            currentGame.Publisher = game.Publisher;

            // Remove Relações para limpar tabela intermediaria
            _dbContext.GamePlatforms.RemoveRange(currentGame.GamePlatforms);
            _dbContext.GameGenres.RemoveRange(currentGame.GameGenres);

            // Adiciona relações
            currentGame.GamePlatforms = game.GamePlatforms;
            currentGame.GameGenres = game.GameGenres;

            _dbContext.Games.Update(currentGame);
            await _dbContext.SaveChangesAsync();

            return currentGame;
        }

        public async Task<GameModel> BuscarPorId(int id)
        {
            var game = await _dbContext.Games
            .Where(g => g.Id == id)
            .Include(g => g.Publisher)
            .Include(g => g.GamePlatforms)
                .ThenInclude(gp => gp.Platform)
            .Include(g => g.GameGenres)
                .ThenInclude(gg => gg.Genre)
            .AsSplitQuery()
            .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null) 
                throw new ArgumentException("Nenhum Jogo encontrado!");

            return game;
        }

        public async Task<List<GameModel>> BuscarTodos()
        {
            List<GameModel> games = await _dbContext.Games
            .Include(g => g.Publisher)
            .Include(g => g.GamePlatforms)
                .ThenInclude(gp => gp.Platform)
            .Include(g => g.GameGenres)
                .ThenInclude(gg => gg.Genre)
            .ToListAsync();
            return games;
        }

        public async Task<bool> Deletar(int id)
        {
            var currentGame = await BuscarPorId(id);
            if (currentGame == null) throw new ArgumentException("Nenhum Jogo encontrado!");
            _dbContext.Games.Remove(currentGame);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<GameRatingModel>> BuscarRatings(int id)
        {
            List<GameRatingModel> gameRatings = await _dbContext.GameRatings
                .Include(gr => gr.Game)
                .Where(gr => gr.GameId == id)
                .Include(gr => gr.User)
                .ToListAsync();

            return gameRatings;
        }
    }
}
