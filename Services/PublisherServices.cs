﻿using LudoVault.Model;
using LudoVault.Repositories.Interfaces;
using LudoVault.Services.Interfaces;
using LudoVault.Services.Mapper;
using LudoVault.Services.Requests;
using LudoVault.Services.Responses;

namespace LudoVault.Services
{
    public class PublisherServices(IPublisherRepository publisherReposiroty) : IPublisherServices
    {
        private readonly IPublisherRepository _publisherRepository = publisherReposiroty;

        // Desenvolvedora
        public async Task<PublisherResponse> CriarPublisherAsync(PublisherRequest publisher)
        {
            var publisherModel = await _publisherRepository.CriarPublisherAsync(PublisherMapper.ToModel(publisher));
            return PublisherMapper.ToResponse(
                publisherModel, 
                publisherModel.Games.Select(GameMapper.ToResponse).ToList());
        }
        public async Task<PublisherResponse> AtualizarPublisherAsync(PublisherRequest publisher, int id)
        {
            var publisherModel = PublisherMapper.ToModel(publisher);
            publisherModel.Id = id;
            var updatedPublisher = await _publisherRepository.AtualizarPublisherAsync(publisherModel);

            return PublisherMapper.ToResponse(
                updatedPublisher,
                updatedPublisher.Games.Select(GameMapper.ToResponse).ToList());
        }
        public async Task<List<PublisherResponse>> BuscarTodasPublishersAsync()
        {
            List<PublisherModel> pubModelList = await _publisherRepository.BuscarTodasPublishersAsync();

            return pubModelList.Select(publisher => PublisherMapper.ToResponse(
                publisher, 
                publisher.Games.Select(GameMapper.ToResponse)
            .ToList())).ToList();
        }
        public async Task<PublisherResponse> BuscarPublisherPorIdAsync(int id)
        {
            var publisherModel = await _publisherRepository.BuscarPublisherPorIdAsync(id);
            return PublisherMapper.ToResponse(
                publisherModel, 
                publisherModel.Games.Select(GameMapper.ToResponse).ToList()
                );
        }
        public async Task<bool> ExcluirPublisherAsync(int id)
        {
            bool publisherExcluded = await _publisherRepository.ExcluirPublisherAsync(id);
            if (publisherExcluded) return true;
            return false;
        }
    }
}
