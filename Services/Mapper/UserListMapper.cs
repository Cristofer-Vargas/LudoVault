﻿using LudoVault.Model;
using LudoVault.Services.Responses;

namespace LudoVault.Services.Mapper
{
    public static class UserListMapper
    {

        public static UserListListsResponse ToListResponse(UserListModel list)
        {
            return new UserListListsResponse
            {
                Id = list.Id,
                ListName = list.Name,
                Games = list.ListItems.Select(g => ToGameResponse(g)).ToList(),
                TotalGames = list.ListItems.Count
            };
        }
        public static UserListGameResponse ToGameResponse(UserListItemsModel listItem)
        {
            return new UserListGameResponse
            {
                GameId = listItem.Game.Id,
                Name = listItem.Game.Name,
                ImageUrl = listItem.Game.ImageUrl,
                PublisherName = listItem.Game.Publisher.Name,
                CreatedAt = listItem.CreatedAt.ToString()
            };
        }
    }
}
