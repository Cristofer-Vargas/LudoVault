using BCrypt.Net;
using LudoVault.Data;
using LudoVault.Model;
using LudoVault.Repositories.Interfaces;

namespace LudoVault.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly MysqlContext _dbContext;
        public UserRepository(MysqlContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<UserModel> Atualizar(UserModel user)
        {
            throw new NotImplementedException();
        }

        public async Task<UserModel> BuscarUsuarioPorId(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserModel> CriarUsuario(UserModel user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> VerificarEmailExistente(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> VerificarIdExistente(long id)
        {
            throw new NotImplementedException();
        }
    }
}
