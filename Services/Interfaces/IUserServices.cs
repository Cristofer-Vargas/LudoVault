﻿using LudoVault.Model;
using LudoVault.Services.Requests;
using LudoVault.Services.Responses;

namespace LudoVault.Services.Interfaces
{
    public interface IUserServices
    {
        public Task<UserResponse> BuscarUsuarioPorId(int id);
        public Task<UserResponse> CriarUsuario(UserRequest user);
        public Task<UserResponse> AtualizarUsuario(UserRequest user, int id);
        public Task<bool> VerificarEmailEmUso(string email);
        public Task<UserRatingListGamesResponse> BuscarUserRatings(int id);
        public Task<UserListResponse> BuscarUserLists(int id);
    }
}
