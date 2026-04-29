﻿using LudoVault.Model;
using LudoVault.Services.Requests;
using LudoVault.Services.Responses;

namespace LudoVault.Services.Mapper
{
    public static class PlatformMapper
    {
        public static PlatformResponse ToResponse(PlatformModel platform)
        {
            return new PlatformResponse()
            {
                Id = platform.Id,
                Name = platform.Name
            };
        }
    }
}
