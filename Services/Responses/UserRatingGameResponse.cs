﻿namespace LudoVault.Services.Responses
{
    public class UserRatingGameResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Rating { get; set; }
        public string? Comment { get; set; }
        public string? CreatedAt { get; set; }
    }
}
