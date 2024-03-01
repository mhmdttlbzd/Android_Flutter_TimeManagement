using BackEnd_WebApi.Application.Dtos;
using BackEnd_WebApi.Application.Exeptions;
using BackEnd_WebApi.Application.Interfaces;
using BackEnd_WebApi.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackEnd_WebApi.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly string SigningKey;
        private readonly string? Audience;
        private readonly string? Issuer;

        public UserService(UserManager<ApplicationUser> userManager, AppSetting appSetting)
        {
            _userManager = userManager;

            SigningKey = appSetting.signingKey;
            Audience = appSetting.Audience;
            Issuer = appSetting.Issuer;
        }
        private JwtSecurityToken GetToken(string userName)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey));
            var clamsList = new List<Claim>
            {
                new Claim(ClaimTypes.Name,userName ),
            };
            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                expires: DateTime.Now.AddMinutes(30),
                claims: clamsList,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }
        public async Task<string> Login(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Email);
            if (user != null)
            {
                var res = await _userManager.CheckPasswordAsync(user, dto.Password);
                if (res)
                {
                    var token = GetToken(user.UserName);
                    var tk = new JwtSecurityTokenHandler().WriteToken(token);
                    return tk;
                }
                throw new NotMachEmailPassExaception();
            }
            throw new IncorrectInputExaception();
        }

        public async Task<string> Register(RegisterInputDto inputDto)
        {
            var user = new ApplicationUser
            {
                Email = inputDto.Email,
                PasswordHash = inputDto.Password,
                UserName = inputDto.Email,
                Name = inputDto.Name,
                Family = inputDto.Family
            };
            var oldUser = await _userManager.FindByNameAsync(inputDto.Email);
            if (oldUser != null)
            {
                throw new DuplicateEmailException();
            }

            var result = await _userManager.CreateAsync(user, inputDto.Password);

            if (result.Succeeded)
            {
                string tk;
                var token = GetToken(user.UserName);
                tk = new JwtSecurityTokenHandler().WriteToken(token);
                return tk;
            }
            throw new IncorrectInputExaception();
        }
    }
}

