﻿using LudoVault.Model;

namespace LudoVault.Repositories.Interfaces
{
    public interface IGameRepository
    {
        public Task<GameModel> CriarGameAsync(GameModel game);
        public Task<List<GameModel>> BuscarTodosGamesAsync();
        public Task<GameModel> BuscarGamePorIdAsync(int id);
        public Task<GameModel> AtualizarGameAsync(GameModel game, int id);
        public Task<bool> DeletarGameAsync(int id);
        public Task<List<GameRatingModel>> BuscarAvaliacoesDoJogoAsync(int id);
    }
}
