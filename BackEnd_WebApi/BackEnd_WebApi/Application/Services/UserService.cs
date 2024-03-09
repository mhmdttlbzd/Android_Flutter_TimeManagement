using BackEnd_WebApi.Application.Dtos;
using BackEnd_WebApi.Application.Exeptions;
using BackEnd_WebApi.Application.Interfaces;
using BackEnd_WebApi.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

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
                expires: DateTime.Now.AddMinutes(10),
                claims: clamsList,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }
        public async Task<LoginResponce> Login(LoginDto dto)
        {
            ApplicationUser? user = null;
            if (dto != null && dto.Email != null)
            {
                user = await _userManager.FindByNameAsync(dto.Email);
            }

            if (user != null && dto?.Password != null)
            {
                var res = await _userManager.CheckPasswordAsync(user, dto.Password);
                if (res)
                {
                    var token = GetToken(user.UserName ?? dto.Email ?? string.Empty);
                    var tk = new JwtSecurityTokenHandler().WriteToken(token);
                    var refreshToken = GenerateRefreshToken();
                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
                    await _userManager.UpdateAsync(user);
                    var result = new LoginResponce { Token = tk, RefreshToken = refreshToken };
                    return result;
                }
                throw new NotMachEmailPassExaception();
            }
            throw new IncorrectInputExaception();
        }

        public async Task<RegisterResponce> Register(RegisterInputDto inputDto)
        {
            var forgotPass = GenerateForgotPass();
            var user = new ApplicationUser
            {
                Email = inputDto.Email,
                PasswordHash = inputDto.Password,
                UserName = inputDto.Email,
                Name = inputDto.Name,
                Family = inputDto.Family,
                ForgotPassword = forgotPass
            };
            var oldUser = await _userManager.FindByNameAsync(inputDto.Email);
            if (oldUser != null)
            {
                throw new DuplicateEmailException();
            }
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            var result = await _userManager.CreateAsync(user, inputDto.Password);

            if (result.Succeeded)
            {
                var tk = GetToken(user.UserName);
                string token = new JwtSecurityTokenHandler().WriteToken(tk);
                var res = new RegisterResponce{
                Token = token,
                RefreshToken = refreshToken,
                ForgotPassword = forgotPass
                };
                return res;
            }
            throw new IncorrectInputExaception();
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        private string GenerateForgotPass()
        {
            var randomNumber = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        public async Task<string> RefreshToken(AuthenticatedResponse input)
        {
            var clames = GetPrincipalFromExpiredToken(input.Token ?? string.Empty);
            var userName = clames.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(input.RefreshToken))
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null && user.RefreshToken == input.RefreshToken && user.RefreshTokenExpiryTime > DateTime.Now)
                {
                    var jToken = GetToken(user.UserName ?? string.Empty);
                    var tk = new JwtSecurityTokenHandler().WriteToken(jToken);
                    return tk;
                }
            }
            throw new IncorrectInputExaception();
        }

        public async Task<bool> ResetPassword(string userName,string newPassword,string oldPassword)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(newPassword) && !string.IsNullOrEmpty(oldPassword))
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null && await _userManager.CheckPasswordAsync(user, oldPassword))
                {
                   var res = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
                    if (res.Succeeded) { return true; }
                }
            }
            throw new IncorrectInputExaception();
        }

        public async Task<bool> ForgetPassword(ForgetPasswordDto input)
        {
            if (input == null) throw new IncorrectInputExaception();
            var user = await _userManager.FindByNameAsync(input.Email);
            if (user == null || user.ForgotPassword != input.ForgotPassword) throw new IncorrectInputExaception();
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, input.NewPassword);
            return true;
        }
    }
}

