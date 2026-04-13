using LudoVault.Model;
using LudoVault.Repositories.Interfaces;

namespace LudoVault.Repositories
{
    public class GameRepository : IGameReposiroty
    {

        public List<GameModel> GameMock = new List<GameModel>
        {
            new GameModel
            {
                Id = 1,
                Name = "Read Dead Redemption 2",
                Description = "Arthur Morgan e a gangue Van der Linde são forçados a fugir. Com agentes federais e caçadores de recompensas no seu encalço, " +
                "a gangue precisa roubar, assaltar e lutar para sobreviver no impiedoso coração dos Estados Unidos.",
                PublisherId = 1,
                CreateAt = DateTime.Now
            },
            new GameModel
            {
                Id = 2,
                Name = "Grand Theft Auto V",
                Description = "Aproveite os fenômenos do entretenimento Grand Theft Auto V e Grand Theft Auto Online melhorados para uma nova geração," +
                " com gráficos deslumbrantes, tempos de carregamento mais rápidos, áudio 3D e mais, além de conteúdo exclusivo para jogadores do GTA Online.",
                PublisherId = 1,
                CreateAt = DateTime.Now
            }
        };

        public GameModel Criar(GameModel game)
        {
            var lastGame = GameMock.Last();
            game.Id = lastGame.Id + 1;
            game.CreateAt = DateTime.Now;
            GameMock.Add(game);
            return game;
        }

        public GameModel Atualizar(GameModel game)
        {
            var gameExisted = GameMock.FirstOrDefault(g => g.Id == game.Id) ?? throw new Exception("Jogo não encontrado para atualização");
            gameExisted.Name = game.Name;
            gameExisted.Image_url = game.Image_url;
            gameExisted.Description = game.Description;
            gameExisted.PublisherId = game.PublisherId;

            return gameExisted;
        }

        public GameModel BuscarPorId(long id)
        {
            var gameExist = GameMock.FirstOrDefault(g => g.Id == id) ?? throw new ArgumentException($"Não foi possível encontrar o jogo desejado");

            return gameExist;
        }

        public List<GameModel> BuscarTodos()
        {
            return GameMock;
        }

        public bool Deletar(long id)
        {
            var gameExist = GameMock.FirstOrDefault(g => g.Id == id);
            if (gameExist != null)
            {
                GameMock.Remove(gameExist);
                return true;
            }

            return false;
        }
    }
}
