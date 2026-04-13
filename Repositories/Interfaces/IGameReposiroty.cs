using LudoVault.Model;

namespace LudoVault.Repositories.Interfaces
{
    public interface IGameReposiroty
    {
        public GameModel Criar(GameModel game);
        public List<GameModel> BuscarTodos();
        public GameModel BuscarPorId(long id);
        public GameModel Atualizar(GameModel game);
        public bool Deletar(long id);
    }
}
