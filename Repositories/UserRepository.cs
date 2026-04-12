using BCrypt.Net;
using LudoVault.Model;
using LudoVault.Repositories.Interfaces;

namespace LudoVault.Repositories
{
    public class UserRepository : IUserRepository
    {
        public List<UserModel> UserMock = new List<UserModel>
        {
            new UserModel
            {
                Id = 1,
                Name = "Cristofer",
                Email = "cristofer@gmail.com",
                Bio = "Bem vindos ao meu perfil!",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teste"),
                CreatedAt = DateTime.Now
            },
            new UserModel
            {
                Id = 2,
                Name = "João Augusto",
                Email = "joaozinho@gmail.com",
                Bio = "Perfil para amantes de GATOS!",
                CreatedAt = DateTime.Now
            }
        };

        public UserModel Atualizar(UserModel user)
        {
            throw new NotImplementedException();
        }

        public UserModel BuscarUsuarioPorId(long id)
        {
            var user = UserMock.FirstOrDefault(x => x.Id == id);
            if (user == null) throw new InvalidOperationException($"Não foi possivel encontrar Usuário com ID: {id}");

            return user;
        }

        public UserModel CriarUsuario(UserModel user)
        {
            var lastPerson = UserMock.Last();
            user.Id = lastPerson.Id + 1;
            UserMock.Add(user);

            return user;
        }

        public bool VerificarEmailExistente(string email)
        {
            var userWithMail = UserMock.FirstOrDefault(e => e.Email.Trim() == email.Trim());
            if (userWithMail == null) return false;
            return true;
        }

        public bool VerificarIdExistente(long id)
        {
            var user = UserMock.FirstOrDefault(x => x.Id == id);
            if (user == null) return false;
            return true;
        }
    }
}
