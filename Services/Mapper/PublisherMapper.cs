using LudoVault.Model;
using LudoVault.Services.Requests;
using LudoVault.Services.Responses;

namespace LudoVault.Services.Mapper
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
                Id = publisherModel.Id.ToString(),
                Name = publisherModel.Name,
                Games = games
            };
        }
    }
}
