namespace LudoVault.Services.Responses
{
    public class UserListResponse
    {
        public List<UserListListsResponse> Lists { get; set; } = [];
        public int? TotalLists { get; set; }
    }
}
