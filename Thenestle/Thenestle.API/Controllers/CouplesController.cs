using Microsoft.AspNetCore.Mvc;
using Thenestle.Domain.Models;
using AutoMapper;
using Thenestle.Domain.Interfaces.Repositories;
using Thenestle.API.Helper;
using Thenestle.API.DTO.User;
using Thenestle.Persistence.Repositories;
using Thenestle.API.DTO.Couples;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Thenestle.Domain.Interfaces.Services;

namespace Thenestle.API.Controllers
{
    /// <summary>
    /// Контроллер для управления парами пользователей.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("99.0")]
    [Route("api/v{version:apiVersion}/couples")]
    [Authorize]
    public class CouplesController : ControllerBase
    {
        private readonly ICoupleRepository _coupleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IInviteRepository _inviteRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;
        private readonly IGeneratorCode _generatorCode;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CouplesController"/>.
        /// </summary>
        /// <param name="coupleRepository">Репозиторий для работы с парами.</param>
        /// <param name="userRepository">Репозиторий для работы с пользователем.</param>
        /// <param name="inviteRepository">Репозиторий для работы с приглошениями.</param>
        /// <param name="mapper">Сервис для маппинга объектов.</param>
        /// <param name="logger">Логгер для записи событий.</param>
        public CouplesController(
            ICoupleRepository coupleRepository,
            IUserRepository userRepository,
            IInviteRepository inviteRepository,
            IMapper mapper,
            ILogger<UsersController> logger,
            IGeneratorCode generatorCode)
        {
            _coupleRepository = coupleRepository;
            _userRepository = userRepository;
            _inviteRepository = inviteRepository;
            _mapper = mapper;
            _logger = logger;
            _generatorCode = generatorCode;
        }

        /// <summary>
        /// Получает список пар с пагинацией.
        /// </summary>
        /// <param name="pageNumber">Номер страницы (по умолчанию 1).</param>
        /// <param name="pageSize">Количество элементов на странице (по умолчанию 10).</param>
        /// <param name="sortField">Поле для сортировки (по умолчанию "CoupleId").</param>
        /// <param name="ascending">Направление сортировки (по умолчанию true - по возрастанию).</param>
        /// <returns>
        /// Возвращает <see cref="ActionResult{PagedResponse{CoupleDTO}}"/>:
        /// - 200 OK с пагинированным списком пар при успешном выполнении.
        /// - 404 Not Found, если пары не найдены.
        /// - 500 Internal Server Error, если произошла ошибка при обработке запроса.
        /// </returns>
        /// <remarks>
        /// ### Пример запроса:
        /// GET /api/v1/couples?pageNumber=1&amp;pageSize=10&amp;sortField=CoupleId&amp;ascending=true
        /// 
        /// ### Пример успешного ответа:
        /// ```json
        /// {
        ///   "data": [
        ///     {
        ///       "coupleId": 1,
        ///       "user1Id": 1,
        ///       "user2Id": 2,
        ///       "createdAt": "2025-06-21T16:01:04.115Z"
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
        ///   "error": "Пары не найдены"
        /// }
        /// ```
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<CoupleDTO>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PagedResponse<CoupleDTO>>> GetCouples(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortField = "CoupleId",
            [FromQuery] bool ascending = true)
        {
            try
            {
                var (couples, totalCount) = await _coupleRepository.GetCouplesAsync(
                        pageNumber, pageSize, sortField, ascending);

                if (couples == null || !couples.Any())
                    return NotFound("пары не найдены");

                return Ok(new PagedResponse<CoupleDTO>(
                    _mapper.Map<List<CoupleDTO>>(couples ?? new List<Couple>()),
                    pageNumber,
                    pageSize,
                    totalCount
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка пар");
                return StatusCode(500, "Произошла ошибка при обработке запроса");
            }
        }

        /// <summary>
        /// Получает пару по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пары.</param>
        /// <returns>
        /// Возвращает <see cref="ActionResult{CoupleDTO}"/>:
        /// - 200 OK с данными о паре при успешном выполнении.
        /// - 404 Not Found, если пара не найден.
        /// - 500 Internal Server Error, если произошла ошибка при обработке запроса.
        /// </returns>
        /// <remarks>
        /// ### Пример запроса:
        /// GET /api/v1/couples/1
        /// 
        /// ### Пример успешного ответа:
        /// ```json
        /// {
        ///       "coupleId": 1,
        ///       "user1Id": 1,
        ///       "user2Id": 2,
        ///       "createdAt": "2025-06-21T16:01:04.115Z"
        /// }
        /// ```
        /// 
        /// ### Пример ошибки:
        /// ```json
        /// {
        ///   "error": "Пара не найдена"
        /// }
        /// ```
        /// </remarks>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PagedResponse<CoupleDTO>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<CoupleDTO>> GetCouple(int id)
        {
            try
            {
                var couple = await _coupleRepository.GetCoupleByIdAsync(id);
                if (couple == null)
                    return NotFound();

                return Ok(_mapper.Map<CoupleDTO>(couple));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении пары с ID {id}");
                return StatusCode(500, "Произошла ошибка при обработке запроса");
            }
        }

        /// <summary>
        /// Создание пары администратором.
        /// </summary>
        [HttpPost("createByAdmin")]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("99.0")]
        public async Task<ActionResult<CreateCoupleDTO>> CreateCoupleByAdmin(int user1Id, int user2Id)
        {
            try
            {
                var user1Exists = await _userRepository.UserExistsByIdAsync(user1Id);
                var user2Exists = await _userRepository.UserExistsByIdAsync(user2Id);

                if (!user1Exists || !user2Exists)
                {
                    return BadRequest("Один или оба пользователя не найдены.");
                }

                var couple = new Couple
                {
                    User1Id = user1Id,
                    User2Id = user2Id,
                    CreatedAt = DateTime.UtcNow,
                };

                await _coupleRepository.AddCoupleAsync(couple);

                var user1 = await _userRepository.GetUserByIdAsync(user1Id);
                var user2 = await _userRepository.GetUserByIdAsync(user2Id);

                user1.Couple = couple;
                user2.Couple = couple;

                await _userRepository.UpdateUserAsync(user1);
                await _userRepository.UpdateUserAsync(user2);

                return CreatedAtAction(nameof(CreateCouple), new { id = couple.CoupleId }, _mapper.Map<CoupleDTO>(couple));

            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, $"Ошибка при создании пары с ID1 {user1Id} и ID2 {user2Id}");
                return StatusCode(500, "Произошла ошибка при обработке запроса");
            }
        }


        /// <summary>
        /// Создание пары по инвайту.
        /// </summary>
        [HttpPost("create")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<CoupleDTO>> CreateCouple()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                // Проверка, есть ли у пользователя уже пара
                var existingCouple = await _coupleRepository.GetCoupleByUserIdAsync(userId);
                if (existingCouple != null)
                {
                    return BadRequest("Вы уже состоите в паре");
                }

                // Создание новой пары
                var couple = new Couple
                {
                    User1Id = userId,
                    User2Id = 0, // Устанавливаем 0 для "неполной" пары
                    CreatedAt = DateTime.UtcNow
                };

                await _coupleRepository.AddCoupleAsync(couple);

                // Генерация кода приглашения
                var inviteCode = _generatorCode.GenerateCodeAsync();
                var invite = new Invite
                {
                    Code = inviteCode,
                    CoupleId = couple.CoupleId,
                    InviterId = userId,
                    ExpiresAt = DateTime.UtcNow.AddDays(7), // Код действителен 7 дней
                    Status = "pending"
                };

                await _inviteRepository.AddInviteAsync(invite);

                // Обновление ссылки на пару у пользователя
                var user = await _userRepository.GetUserByIdAsync(userId);
                user.CoupleId = couple.CoupleId;
                await _userRepository.UpdateUserAsync(user);

                var coupleDto = _mapper.Map<CoupleDTO>(couple);
                coupleDto.InviteCode = inviteCode; // Возвращаем код в ответе
                return Ok(coupleDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании пары");
                return StatusCode(500, "Ошибка при создании пары");
            }
        }

        [HttpPost("{coupleId}/complete")]
        public async Task<ActionResult<CoupleDTO>> CompleteCouple(int coupleId, [FromBody] int secondUserId)
        {
            try
            {
                var couple = await _coupleRepository.GetCoupleByIdAsync(coupleId);
                if (couple == null)
                {
                    return NotFound("Couple not found");
                }

                // Check if second user already has a couple
                var secondUser = await _userRepository.GetUserByIdAsync(secondUserId);
                if (secondUser.CoupleId != null)
                {
                    return BadRequest("This user already has a couple");
                }

                // Update couple with second user
                couple.User2Id = secondUserId;
                await _coupleRepository.UpdateCoupleAsync(couple);

                // Update second user's couple reference
                secondUser.CoupleId = couple.CoupleId;
                await _userRepository.UpdateUserAsync(secondUser);

                return Ok(_mapper.Map<CoupleDTO>(couple));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing couple");
                return StatusCode(500, "Error completing couple");
            }
        }

        [HttpGet("my")]
        [ProducesResponseType(typeof(CoupleDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CoupleDTO>> GetMyCouple()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized("Пользователь не аутентифицирован");

                int userId = int.Parse(userIdClaim.Value);

                var couple = await _coupleRepository.GetCoupleByUserIdAsync(userId);
                if (couple == null)
                    return NotFound("Пара не найдена");

                return Ok(_mapper.Map<CoupleDTO>(couple));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении пары пользователя");
                return StatusCode(500, "Произошла ошибка при обработке запроса");
            }
        }

        /// <summary>
        /// Удаляет пару.
        /// </summary>
        /// <param name="id">Идентификатор пары.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 204 No Content при успешном удалении.
        /// - 404 Not Found, если пара не найдена.
        /// - 500 Internal Server Error, если произошла ошибка при удалении.
        /// </returns>
        /// <remarks>
        /// ### Пример запроса:
        /// DELETE /api/v1/couples/1
        /// 
        /// ### Пример успешного ответа:
        /// HTTP 204 No Content
        /// 
        /// ### Пример ошибки:
        /// ```json
        /// {
        ///   "error": "Пара не найдена"
        /// }
        /// ```
        /// </remarks>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCouple(int id)
        {
            try
            {
                var user = await _coupleRepository.GetCoupleByIdAsync(id);
                if (user == null)
                    return NotFound();

                await _coupleRepository.DeleteCoupleAsync(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении пары с ID {id}");
                return StatusCode(500, "Произошла ошибка при удалении пары");
            }
        }
    }
}
