using Microsoft.AspNetCore.Mvc;
using Thenestle.Domain.Models;
using AutoMapper;
using Thenestle.Domain.Interfaces.Repositories;
using Thenestle.API.Helper;
using Thenestle.API.DTO.User;
using Microsoft.AspNetCore.Authorization;

namespace Thenestle.API.Controllers
{
    /// <summary>
    /// Контроллер для управления пользователями системы.
    /// </summary>
    [Route("api/v{version:apiVersion}/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="UsersController"/>.
        /// </summary>
        /// <param name="userRepository">Репозиторий для работы с пользователями.</param>
        /// <param name="mapper">Сервис для маппинга объектов.</param>
        /// <param name="logger">Логгер для записи событий.</param>
        public UsersController(
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<UsersController> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Получает список пользователей с пагинацией.
        /// </summary>
        /// <param name="pageNumber">Номер страницы (по умолчанию 1).</param>
        /// <param name="pageSize">Количество элементов на странице (по умолчанию 10).</param>
        /// <param name="sortField">Поле для сортировки (по умолчанию "UserId").</param>
        /// <param name="ascending">Направление сортировки (по умолчанию true - по возрастанию).</param>
        /// <returns>
        /// Возвращает <see cref="ActionResult{PagedResponse{UserDto}}"/>:
        /// - 200 OK с пагинированным списком пользователей при успешном выполнении.
        /// - 404 Not Found, если пользователи не найдены.
        /// - 500 Internal Server Error, если произошла ошибка при обработке запроса.
        /// </returns>
        /// <remarks>
        /// ### Пример запроса:
        /// GET /api/v1/users?pageNumber=1&amp;pageSize=10&amp;sortField=LastName&amp;ascending=true
        /// 
        /// ### Пример успешного ответа:
        /// ```json
        /// {
        ///   "data": [
        ///     {
        ///       "userId": 1,
        ///       "firstName": "John",
        ///       "lastName": "Doe",
        ///       "email": "john.doe@example.com",
        ///       "gender": "Female",
        ///       "createdAt": "2025-06-21T16:11:45.112904Z",
        ///       "currencyBalance": 0
        ///     }
        ///   ],
        ///   "pageNumber": 1,
        ///   "pageSize": 10,
        ///   "totalCount": 100
        /// }
        /// ```
        /// 
        /// ### Пример ошибки:
        /// ```json
        /// {
        ///   "error": "Пользователи не найдены"
        /// }
        /// ```
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<UserDTO>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PagedResponse<UserDTO>>> GetUsers(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortField = "UserId",
            [FromQuery] bool ascending = true)
        {
            try
            {
                var (users, totalCount) = await _userRepository.GetUsersAsync(
                    pageNumber, pageSize, sortField, ascending);

                if (users == null || !users.Any())
                    return NotFound("Пользователи не найдены");

                var userDtos = _mapper.Map<List<UserDTO>>(users);

                return Ok(new PagedResponse<UserDTO>(userDtos, pageNumber, pageSize, totalCount));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка пользователей");
                return StatusCode(500, "Произошла ошибка при обработке запроса");
            }
        }

        /// <summary>
        /// Получает пользователя по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>
        /// Возвращает <see cref="ActionResult{UserDto}"/>:
        /// - 200 OK с данными пользователя при успешном выполнении.
        /// - 404 Not Found, если пользователь не найден.
        /// - 500 Internal Server Error, если произошла ошибка при обработке запроса.
        /// </returns>
        /// <remarks>
        /// ### Пример запроса:
        /// GET /api/v1/users/1
        /// 
        /// ### Пример успешного ответа:
        /// ```json
        /// {
        ///   "userId": 1,
        ///   "firstName": "John",
        ///   "lastName": "Doe",
        ///   "email": "john.doe@example.com"
        /// }
        /// ```
        /// 
        /// ### Пример ошибки:
        /// ```json
        /// {
        ///   "error": "Пользователь не найден"
        /// }
        /// ```
        /// </remarks>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PagedResponse<UserDTO>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound();

                return Ok(_mapper.Map<UserDTO>(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении пользователя с ID {id}");
                return StatusCode(500, "Произошла ошибка при обработке запроса");
            }
        }

        /// <summary>
        /// Обновляет данные пользователя.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <param name="updateUserDto">DTO с обновленными данными пользователя.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 204 No Content при успешном обновлении.
        /// - 400 Bad Request, если модель данных невалидна.
        /// - 404 Not Found, если пользователь не найден.
        /// - 500 Internal Server Error, если произошла ошибка при обновлении.
        /// </returns>
        /// <remarks>
        /// ### Пример запроса:
        /// PUT /api/v1/users/1
        /// ```json
        /// {
        ///   "firstName": "John",
        ///   "lastName": "Smith",
        ///   "email": "john.smith@example.com"
        /// }
        /// ```
        /// 
        /// ### Пример успешного ответа:
        /// HTTP 204 No Content
        /// 
        /// ### Пример ошибки:
        /// ```json
        /// {
        ///   "error": "Пользователь не найден"
        /// }
        /// ```
        /// </remarks>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDTO updateUserDto)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound();

                _mapper.Map(updateUserDto, user);
                await _userRepository.UpdateUserAsync(user);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при обновлении пользователя с ID {id}");
                return StatusCode(500, "Произошла ошибка при обновлении пользователя");
            }
        }

        /// <summary>
        /// Удаляет пользователя.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 204 No Content при успешном удалении.
        /// - 404 Not Found, если пользователь не найден.
        /// - 500 Internal Server Error, если произошла ошибка при удалении.
        /// </returns>
        /// <remarks>
        /// ### Пример запроса:
        /// DELETE /api/v1/users/1
        /// 
        /// ### Пример успешного ответа:
        /// HTTP 204 No Content
        /// 
        /// ### Пример ошибки:
        /// ```json
        /// {
        ///   "error": "Пользователь не найден"
        /// }
        /// ```
        /// </remarks>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound();

                await _userRepository.DeleteUserAsync(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении пользователя с ID {id}");
                return StatusCode(500, "Произошла ошибка при удалении пользователя");
            }
        }
    }
}
