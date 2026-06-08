using LudoVault.Application.DTO.Requests;
using LudoVault.Application.DTO.Responses;
using LudoVault.Domain.Model;

namespace LudoVault.Application.Services.Mapper
{
  public static class PublisherMapper
  {
    public static PublisherModel ToModel(PublisherRequest publisherRequest)
    {
      return new PublisherModel()
      {
        Name = publisherRequest.Name
      };
    }

    public static PublisherResponse ToResponse(PublisherModel publisherModel, List<GameResponse> games)
    {
      return new PublisherResponse()
      {
        Id = publisherModel.Id,
        Name = publisherModel.Name,
        Games = games
      };
    }
  }
}
