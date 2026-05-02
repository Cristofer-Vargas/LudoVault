using BCrypt.Net;
using LudoVault.Data;
using LudoVault.Model;
using LudoVault.Repositories.Interfaces;
using LudoVault.Services.Requests;
using Microsoft.EntityFrameworkCore;

namespace LudoVault.Repositories
{
    public class UserRepository(MysqlContext dbContext) : IUserRepository
    {
        public readonly MysqlContext _dbContext = dbContext;
        
        // Usuário
        public async Task<UserModel> AtualizarUsuarioAsync(UserModel userUpdate, int id)
        {
            var userExisted = await BuscarUsuarioPorIdAsync(id);
            _dbContext.Users.Entry(userExisted).CurrentValues.SetValues(userUpdate);
            await _dbContext.SaveChangesAsync();

            return userExisted;
        }
        public async Task<UserModel> CriarUsuarioAsync(UserModel user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
        public async Task<UserModel> BuscarUsuarioPorIdAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null) throw new ArgumentException("Nenhum usuário encontrado!");
            return user;
        }
        public async Task<bool> VerificarEmailExistenteAsync(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email);
        }

        // Listas de Usuário
        public async Task<UserListModel> CriarUserListAsync(UserListModel userList)
        {
            await _dbContext.UserLists.AddAsync(userList);    
            await _dbContext.SaveChangesAsync();

            return await _dbContext.UserLists
                .AsNoTracking()
                .FirstOrDefaultAsync(ul => ul.Id == userList.Id) 
                ?? throw new Exception("Erro ao recuperar a lista criada.");
        }
        public async Task<UserListModel> AtualizarUserListAsync(UserListModel userList, int userId, int listId)
        {
            var currentUserList = await _dbContext.UserLists.FindAsync(listId);
            if (currentUserList == null) throw new Exception("Erro ao recuperar a lista informada.");

            userList.Id = currentUserList.Id;
            userList.CreatedAt = currentUserList.CreatedAt;
            userList.UserId = userId;

            _dbContext.UserLists.Entry(currentUserList).CurrentValues.SetValues(userList);
            await _dbContext.SaveChangesAsync();

            return await _dbContext.UserLists
                .Include(ul => ul.ListItems)
                    .ThenInclude(lg => lg.Game)
                        .ThenInclude(g => g.Publisher)
                .FirstOrDefaultAsync(ul => ul.Id == listId)
                ?? throw new Exception("Erro ao recuperar a lista atualizada!");
        }
        public async Task<UserListModel> AdicionarJogoAListaAsync(UserListGameModel userGameList)
        {
            await _dbContext.UserListsItems.AddAsync(userGameList);
            await _dbContext.SaveChangesAsync();

            return await _dbContext.UserLists
                .Include(ul => ul.ListItems)
                    .ThenInclude(lg => lg.Game)
                        .ThenInclude(g => g.Publisher)
                .FirstOrDefaultAsync(ul => ul.Id == userGameList.ListId)
                ?? throw new Exception("Erro ao recuperar a lista de game adicionado!");
        }
        public async Task<UserListGameModel> BuscarItemDaListaAsync(int userListGameId)
        {
            return await _dbContext.UserListsItems.FindAsync(userListGameId) 
                ?? throw new ArgumentException("Erro ao buscar por jogo da lista!");
        }
        public async Task<List<UserListModel>> BuscarUserListsPorUsuarioAsync(int userId)
        {
            return await _dbContext.UserLists
                .AsNoTracking()
                .Include(ul => ul.ListItems)
                    .ThenInclude(uli => uli.Game)
                        .ThenInclude(g => g.Publisher)
                .Where(ul => ul.UserId == userId)
                .AsSplitQuery()
                .ToListAsync();
        }
        public async Task<bool> ExisteListaComMesmoNomeAsync(string name, int userId)
        {
            return await _dbContext.UserLists.AnyAsync(ul => ul.Name == name && ul.UserId == userId);
        }
        public async Task<bool> ExisteUserListAsync(int listId, int userId)
        {
            return await _dbContext.UserLists.AnyAsync(ul => ul.Id == listId && ul.UserId == userId);
        }
        public async Task<bool> JogoExisteNaListaAsync(int gameId, int listId)
        {
            return await _dbContext.UserListsItems.AnyAsync(uli => uli.GameId == gameId && uli.ListId == listId);
        }
        public async Task<bool> DeletarUserListAsync(int listId)
        {
            var userListToDelete = await _dbContext.UserLists.FindAsync(listId);
            if (userListToDelete == null) return false;

            _dbContext.UserLists.Remove(userListToDelete);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RemoverJogoDaListaAsync(int listId, int gameId)
        {
            var gameToDelete = await _dbContext.UserListsItems
                .FirstOrDefaultAsync(uli => uli.ListId == listId && uli.GameId == gameId);

            if (gameToDelete == null) return false;

            _dbContext.UserListsItems.Remove(gameToDelete);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        // Avaliações de Usuário
        public async Task<List<GameRatingModel>> BuscarAvaliacoesDoUsuarioAsync(int userId)
        {
            return await _dbContext.GameRatings
                .Include(gr => gr.Game)
                .Include(gr => gr.User)
                .Where(gr => gr.UserId == userId)
                .ToListAsync();
        }
    }
}
