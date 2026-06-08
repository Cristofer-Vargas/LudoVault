using LudoVault.Application.DTO.Requests;
using LudoVault.Application.DTO.Responses;
using LudoVault.Domain.Model;

namespace LudoVault.Application.Services.Mapper
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

    public static UserListItemModel ToUserListItemModel(int listId, int gameId)
    {
      return new UserListItemModel
      {
        ListId = listId,
        GameId = gameId
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
    public static UserListGameResponse ToGameResponse(UserListItemModel listItem)
    {
      return new UserListGameResponse
      {
        Id = listItem.Id,
        GameId = listItem.Game.Id,
        Name = listItem.Game.Name,
        ImageUrl = listItem.Game.ImageUrl,
        PublisherName = listItem.Game.Publisher.Name,
        CreatedAt = listItem.AddedAt.ToString()
      };
    }
  }
}
