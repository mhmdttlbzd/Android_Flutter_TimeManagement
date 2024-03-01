using BackEnd_WebApi.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace BackEnd_WebApi
{
    public class AppSetting
    {

        public readonly string signingKey;
        public readonly string? Audience;
        public readonly string? Issuer;

        

        public AppSetting( IConfiguration configuration)
        {

            signingKey = configuration["JWTConfig:SigningKey"];
            Audience = configuration["JWTConfig:Audience"];
            Issuer = configuration["JWTConfig:Issuer"];
        }
        
            
        
    }
}
