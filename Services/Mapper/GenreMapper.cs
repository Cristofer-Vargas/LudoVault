using LudoVault.DTO.Responses;
using LudoVault.Model;

namespace LudoVault.Services.Mapper
{
    public static class GenreMapper
    {
        public static GenreResponse ToResponse(GenreModel genre)
        {
            return new GenreResponse
            {
                Id = genre.Id,
                Name = genre.Name,
            };
        }
    }
}
