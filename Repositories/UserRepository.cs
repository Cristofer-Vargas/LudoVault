using BCrypt.Net;
using LudoVault.Data;
using LudoVault.Model;
using LudoVault.Repositories.Interfaces;
using LudoVault.Services.Requests;
using Microsoft.EntityFrameworkCore;

namespace LudoVault.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly MysqlContext _dbContext;
        public UserRepository(MysqlContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<UserModel> Atualizar(UserModel userUpdate, long id)
        {
            // Futuramente fazer verificação com autenticação para Edições a usuários autorizados (dono do perfil)
            var userExisted = await BuscarUsuarioPorId(userUpdate.Id);
            _dbContext.Users.Entry(userExisted).CurrentValues.SetValues(userUpdate);
            await _dbContext.SaveChangesAsync();

            var userNew = await _dbContext.Users
                .FindAsync(userExisted.Id);
            if (userExisted == null) throw new ArgumentException("Não foi encontrado usuário com id passado!");

            return userExisted;
        }

        public async Task<UserModel> BuscarUsuarioPorId(long id)
        {
            var user = await _dbContext.Users
                .FindAsync(id);
            if (user == null) throw new ArgumentException("Nenhum usuário encontrado!");
            
            return user;
        }

        public async Task<UserModel> CriarUsuario(UserModel user)
        {
            await _dbContext.Users
                .AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<bool> VerificarEmailExistente(string email)
        {
            var userWithExistedEmail = await _dbContext.Users
                .Where(u => u.Email == email)
                .ToListAsync();

            if (userWithExistedEmail.Count != 0) return true;

            return false; // Se a lista de users nao houver dados, significa que ninguem esta usando esse email. VerificarEmailExistente retorna false para nao usado
        }
    }
}
