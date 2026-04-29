using LudoVault.Model;

namespace LudoVault.Repositories.Interfaces
{
    public interface IGameRepository
    {
        public Task<GameModel> Criar(GameModel game);
        public Task<List<GameModel>> BuscarTodos();
        public Task<GameModel> BuscarPorId(int id);
        public Task<GameModel> Atualizar(GameModel game, int id);
        public Task<bool> Deletar(int id);
        public Task<List<GameRatingModel>> BuscarRatings(int id);
    }
}
