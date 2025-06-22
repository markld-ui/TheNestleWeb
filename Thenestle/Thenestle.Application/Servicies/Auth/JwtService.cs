using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using Thenestle.Domain.Models;

namespace Thenestle.Application.Servicies.Auth
{
    /// <summary>
    /// Сервис для генерации JWT-токенов.
    /// </summary>
    public class JwtService
    {
        #region Поля

        private readonly IConfiguration _configuration;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="JwtService"/>.
        /// </summary>
        /// <param name="configuration">Конфигурация приложения, содержащая настройки JWT.</param>
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region Методы

        #region GenerateToken
        /// <summary>
        /// Генерирует JWT-токен для пользователя.
        /// </summary>
        /// <param name="user">Пользователь, для которого создается токен.</param>
        /// <returns>Строка, представляющая JWT-токен.</returns>
        public string GenerateToken(Users user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in user.UserRoles.Select(ur => ur.Role).ToList())
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"])),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        #endregion

        #region GenerateRefreshToken
        /// <summary>
        /// Генерирует refresh-токен.
        /// </summary>
        /// <returns>Строка, представляющая refresh-токен.</returns>
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        #endregion

        #region GetPrincipalFromExpiredToken
        /// <summary>
        /// Получает данные пользователя из истекшего токена.
        /// </summary>
        /// <param name="token">Истекший JWT-токен.</param>
        /// <returns>Объект <see cref="ClaimsPrincipal"/>, содержащий данные пользователя.</returns>
        /// <exception cref="SecurityTokenException">Выбрасывается, если токен невалиден.</exception>
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
        #endregion

        #endregion
    }
}
