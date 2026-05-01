﻿using LudoVault.Model;
using LudoVault.Services.Requests;
using LudoVault.Services.Responses;

namespace LudoVault.Services.Mapper
{
    public static class UserListMapper
    {
        public static UserListModel ToUserListModel(UserListRequest userList, int userId)
        {
            return new UserListModel
            {
                Name = userList.Name,
                UserId = userId
            };
        }

        public static UserListGameModel ToUserListGameModel(UserListGameRequest game, int listId)
        {
            return new UserListGameModel
            {
                ListId = game.ListId,
                GameId = game.GameId
            };
        }

        public static UserListListsResponse ToListGameResponse(UserListModel list)
        {
            return new UserListListsResponse
            {
                Id = list.Id,
                ListName = list.Name,
                Games = list.ListItems.Select(g => ToGameResponse(g)).ToList(),
                TotalGames = list.ListItems.Count
            };
        }
        public static UserListGameResponse ToGameResponse(UserListGameModel listItem)
        {
            return new UserListGameResponse
            {
                Id = listItem.Id,
                GameId = listItem.Game.Id,
                Name = listItem.Game.Name,
                ImageUrl = listItem.Game.ImageUrl,
                PublisherName = listItem.Game.Publisher.Name,
                CreatedAt = listItem.CreatedAt.ToString()
            };
        }
    }
}
