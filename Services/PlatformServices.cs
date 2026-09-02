using LudoVault.DTO.Responses;
using LudoVault.Services.Interfaces;
using LudoVault.Services.Mapper;
using LudoVault.Validations.Base;
using LudoVault.Repositories.Interfaces;

namespace LudoVault.Services
{
  public class PlatformServices(IPlatformRepository platformRepo, ILogger<PlatformServices> logger) : IPlatformServices
  {
    private readonly IPlatformRepository _platformRepository = platformRepo;
    private readonly ILogger<PlatformServices> _logger = logger;
    public async Task<Response<List<PlatformResponse>>> BuscarPlataformas()
    {
      var response = new Response<List<PlatformResponse>>();
      var platformsModels = await _platformRepository.BuscarTodos();
      if (platformsModels == null || platformsModels.Count == 0)
      {
        _logger.LogWarning("Erro interno ou nenhuma plataforma cadastrada!");
        response.Report.Add(Report.Create("Erro interno ou nenhuma plataforma cadastrada!", 404));
        return response;
      }

      response.Data = platformsModels.Select(p => PlatformMapper.ToResponse(p)).ToList();
      response.Status = 200;
      return response;
    }
  }
}