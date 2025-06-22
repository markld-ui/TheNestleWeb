using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Thenestle.API.DTO.Auth;
using Thenestle.Application.Servicies.Auth;
using Thenestle.Domain.Interfaces.Auth;
using Thenestle.Domain.Models;
using Thenestle.Domain.Interfaces.Repositories;

namespace Thenestle.API.Controllers
{
    #region Класс AuthController

    /// <summary>
    /// Контроллер для управления аутентификацией и регистрацией пользователей.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("99.0")]
    [ApiController]
    public class AuthController : Controller
    {
        #region Поля и свойства

        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<AuthController> _logger;
        private const int _refreshTokenExpiryDays = 7;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AuthController"/>.
        /// </summary>
        /// <param name="userRepository">Репозиторий для работы с пользователями.</param>
        /// <param name="jwtService">Сервис для генерации JWT-токенов.</param>
        /// <param name="passwordHasher">Сервис для хеширования и проверки паролей.</param>
        /// /// <param name="logger">Сервис для логирования событий.</param>
        public AuthController(
            IUserRepository userRepository,
            JwtService jwtService,
            IPasswordHasher passwordHasher,
            ILogger<AuthController> logger)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        #endregion

        #region Методы

        #region Register

        /// <summary>
        /// Регистрирует нового пользователя.
        /// </summary>
        /// <param name="model">Модель данных для регистрации, содержащая имя пользователя, email и пароль.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 200 OK с токеном и refresh-токеном при успешной регистрации.
        /// - 400 BadRequest, если модель данных невалидна или пользователь с таким именем/email уже существует.
        /// - 500 InternalServerError, если произошла ошибка при регистрации.
        /// </returns>
        /// <remarks>
        /// ### Пример запроса:
        /// POST /api/users/register
        /// ```json
        /// {
        ///   "username": "newuser",
        ///   "email": "newuser@example.com",
        ///   "password": "securePassword123"
        /// }
        /// ```
        /// 
        /// ### Пример успешного ответа:
        /// ```json
        /// {
        ///   "token": "accessTokenString",
        ///   "refreshToken": "refreshTokenString"
        /// }
        /// ```
        /// 
        /// ### Пример ошибки:
        /// ```json
        /// {
        ///   "error": "Пользователь с таким email уже существует."
        /// }
        /// ```
        /// </remarks>
        [HttpPost("register")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (await _userRepository.UserExistsByEmailAsync(model.Email))
                    return BadRequest("Пользователь с таким email уже существует.");

                var refreshToken = _jwtService.GenerateRefreshToken();

                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PasswordHash = _passwordHasher.Generate(model.Password),
                    CurrencyBalance = 0,
                    Gender = model.Gender,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays)
                };

                await _userRepository.AddUserAsync(user);

                var token = _jwtService.GenerateToken(user);

                return Ok(new
                {
                    Token = token,
                    RefreshToken = refreshToken
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при регистрации пользователя.");
                return StatusCode(500, "Произошла ошибка при регистрации.");
            }
        }

        #endregion

        #region Login

        /// <summary>
        /// Аутентифицирует пользователя и возвращает JWT-токен.
        /// </summary>
        /// <param name="model">Модель данных для входа, содержащая email и пароль.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 200 OK с JWT-токеном в случае успешной аутентификации.
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 401 Unauthorized, если пользователь не найден или пароль неверен.
        /// - 500 Internal Server Error, если произошла ошибка в запросе.
        /// </returns>
        /// <remarks>
        /// ### Пример запроса:
        /// POST /api/users/login
        /// ```json
        /// {
        ///   "email": "user@example.com",
        ///   "password": "securePassword123"
        /// }
        /// ```
        /// 
        /// ### Пример успешного ответа:
        /// ```json
        /// {
        ///   "token": "jwtAccessTokenString",
        ///   "refreshToken": "refreshTokenString"
        /// }
        /// ```
        /// 
        /// ### Пример ошибки:
        /// ```json
        /// {
        ///   "error": "Неверный email или пароль."
        /// }
        /// ```
        /// </remarks>
        [HttpPost("login")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userRepository.GetUserByEmailAsync(model.Email);
            if (user == null || !_passwordHasher.Verify(model.Password, user.PasswordHash))
            {
                _logger.LogWarning("Неудачная попытка входа для email: {Email}", model.Email);
                return Unauthorized();
            }

            var token = _jwtService.GenerateToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            await _userRepository.UpdateUserAsync(user);

            return Ok(new
            {
                Token = token,
                RefreshToken = refreshToken
            });
        }

        #endregion

        #region ChangePassword

        /// <summary>
        /// Изменяет пароль пользователя.
        /// </summary>
        /// <param name="model">Модель данных для изменения пароля.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 200 OK при успешном изменении пароля.
        /// - 400 BadRequest, если модель данных невалидна или текущий пароль неверен.
        /// - 401 Unauthorized, если пользователь не аутентифицирован.
        /// - 500 InternalServerError, если произошла ошибка при изменении пароля.
        /// </returns>
        /// <remarks>
        /// ### Пример запроса:
        /// POST /api/auth/change-password
        /// ```json
        /// {
        ///   "currentPassword": "oldPassword123",
        ///   "newPassword": "newSecurePassword456"
        /// }
        /// ```
        /// 
        /// ### Пример успешного ответа:
        /// ```json
        /// {
        ///   "message": "Пароль успешно изменен"
        /// }
        /// ```
        /// 
        /// ### Пример ошибки:
        /// ```json
        /// {
        ///   "error": "Текущий пароль неверен"
        /// }
        /// ```
        /// </remarks>
        [Authorize]
        [HttpPost("change-password")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)
                    return Unauthorized();

                if (!_passwordHasher.Verify(model.CurrentPassword, user.PasswordHash))
                {
                    _logger.LogWarning("Неверная попытка изменения пароля для пользователя с ID: {UserId}", userId);
                    return BadRequest("Текущий пароль неверный!");
                }

                user.PasswordHash = _passwordHasher.Generate(model.NewPassword);
                await _userRepository.UpdateUserAsync(user);

                _logger.LogInformation("Пароль успешно изменен для пользователя с ID: {UserId}", userId);
                return Ok(new { message = "Пароль успешно изменен" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при изменении пароля");
                return StatusCode(500, "Произошла ошибка при изменении пароля");
            }
        }
        #endregion

        #region Refresh

        /// <summary>
        /// Обновляет JWT-токен и refresh token.
        /// </summary>
        /// <param name="model">Модель данных, содержащая текущий JWT-токен и refresh token.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 200 OK с новым JWT-токеном и refresh token в случае успешного обновления.
        /// - 400 BadRequest, если токен невалиден или срок действия refresh token истек.
        /// </returns>
        /// <remarks>
        /// ### Пример запроса:
        /// POST /api/users/refresh
        /// ```json
        /// {
        ///   "token": "expiredJwtTokenString",
        ///   "refreshToken": "currentRefreshTokenString"
        /// }
        /// ```
        /// 
        /// ### Пример успешного ответа:
        /// ```json
        /// {
        ///   "token": "newJwtTokenString",
        ///   "refreshToken": "newRefreshTokenString"
        /// }
        /// ```
        /// 
        /// ### Пример ошибки:
        /// ```json
        /// {
        ///   "error": "Invalid token"
        /// }
        /// ```
        /// </remarks>
        [HttpPost("refresh")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO model)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(model.Token);
            var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier).Value);

            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                _logger.LogWarning("Неудачная попытка обновления токена для пользователя с ID: {UserId}", userId);
                return BadRequest("Invalid token");
            }

            var newToken = _jwtService.GenerateToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            await _userRepository.UpdateUserAsync(user);

            return Ok(new
            {
                Token = newToken,
                RefreshToken = newRefreshToken
            });
        }
        #endregion

        #endregion
    }

    #endregion
}
