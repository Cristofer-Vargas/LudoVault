using LudoVault.Model;

namespace LudoVault.Repositories.Interfaces
{
    public interface IGameRepository
    {
        public Task<GameModel> Criar(GameModel game);
        public Task<List<GameModel>> BuscarTodos();
        public Task<GameModel> BuscarPorId(long id);
        public Task<GameModel> Atualizar(GameModel game, long id);
        public Task<bool> Deletar(long id);
        public Task<List<GameRatingModel>> BuscarRatings(long id);
    }
}
