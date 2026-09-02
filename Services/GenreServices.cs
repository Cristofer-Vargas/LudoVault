using LudoVault.DTO.Responses;
using LudoVault.Services.Interfaces;
using LudoVault.Services.Mapper;
using LudoVault.Validations.Base;
using LudoVault.Repositories.Interfaces;

namespace LudoVault.Services
{
  public class GenreServices(IGenreRepository genreRepo, ILogger<GenreServices> logger) : IGenreServices
  {
    private readonly IGenreRepository _genreRepository = genreRepo;
    private readonly ILogger<GenreServices> _logger = logger;
    public async Task<Response<List<GenreResponse>>> BuscarGeneros()
    {
      var response = new Response<List<GenreResponse>>();
      var genresModels = await _genreRepository.BuscarTodos();
      if (genresModels == null || genresModels.Count == 0)
      {
        _logger.LogWarning("Erro interno ou nenhum gênero cadastrado!");
        response.Report.Add(Report.Create("Erro interno ou nenhum gênero cadastrado!", 404));
        return response;
      }

      response.Data = genresModels.Select(p => GenreMapper.ToResponse(p)).ToList();
      response.Status = 200;
      return response;
    }
  }
}