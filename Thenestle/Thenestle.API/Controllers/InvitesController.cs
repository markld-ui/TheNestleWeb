using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thenestle.API.DTO.Invites;
using Thenestle.Domain.Interfaces.Repositories;
using Thenestle.Domain.Interfaces.Services;
using Thenestle.Domain.Models;

namespace Thenestle.API.Controllers
{
    [Route("api/v{version:apiVersion}/invites")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class InvitesController : ControllerBase
    {
        private readonly IInviteRepository _inviteRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<InvitesController> _logger;
        private readonly IGeneratorCode _generatorCode;

        public InvitesController(
            IInviteRepository inviteRepository,
            IMapper mapper,
            ILogger<InvitesController> logger,
            IGeneratorCode generatorCode)
        {
            _inviteRepository = inviteRepository;
            _mapper = mapper;
            _logger = logger;
            _generatorCode = generatorCode;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InviteDTO>> GetInvite(int id)
        {
            try
            {
                var invite = await _inviteRepository.GetInviteByIdAsync(id);
                if (invite == null)
                    return NotFound("Инвайт не найден");

                return Ok(_mapper.Map<InviteDTO>(invite));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении инвайта с ID {id}");
                return StatusCode(500, "Произошла ошибка при обработке запроса");
            }
        }

        [HttpPost]
        public async Task<ActionResult<InviteDTO>> CreateInvite([FromBody] InviteDTO inviteDto)
        {
            try
            {
                // Получаем идентификатор текущего пользователя из токена
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized("Пользователь не аутентифицирован");

                int inviterId = int.Parse(userIdClaim.Value);

                // Генерируем код (предполагается, что GenerateCodeAsync возвращает Task<string>)
                string code = _generatorCode.GenerateCodeAsync();

                // Проверяем и удаляем уже принятые инвайты с таким же кодом
                var existingInvites = await _inviteRepository.GetInvitesByCodeAsync(code);
                foreach (var existingInvite in existingInvites)
                {
                    if (existingInvite.Status == "accepted")
                    {
                        await _inviteRepository.DeleteInviteAsync(existingInvite);
                    }
                }

                // Создаём новый инвайт
                var invite = _mapper.Map<Invite>(inviteDto);
                invite.InviterId = inviterId;
                invite.Code = code;
                invite.CreatedAt = DateTime.UtcNow;
                invite.IsUsed = false;
                invite.Status = "pending";
                invite.ExpiresAt = DateTime.UtcNow.AddMinutes(4);

                await _inviteRepository.AddInviteAsync(invite);

                return CreatedAtAction(nameof(GetInvite), new { id = invite.InviteId }, _mapper.Map<InviteDTO>(invite));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании инвайта");
                return StatusCode(500, "Произошла ошибка при обработке запроса");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvite(int id, [FromBody] InviteDTO inviteDto)
        {
            try
            {
                var invite = await _inviteRepository.GetInviteByIdAsync(id);
                if (invite == null)
                    return NotFound("Инвайт не найден");

                _mapper.Map(inviteDto, invite);
                await _inviteRepository.UpdateInviteAsync(invite);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при обновлении инвайта с ID {id}");
                return StatusCode(500, "Произошла ошибка при обработке запроса");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvite(int id)
        {
            try
            {
                var invite = await _inviteRepository.GetInviteByIdAsync(id);
                if (invite == null)
                    return NotFound("Инвайт не найден");

                await _inviteRepository.DeleteInviteAsync(invite);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении инвайта с ID {id}");
                return StatusCode(500, "Произошла ошибка при обработке запроса");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InviteDTO>>> GetInvites([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var invites = await _inviteRepository.GetInvitesAsync(pageNumber, pageSize);
                var invitesDto = _mapper.Map<IEnumerable<InviteDTO>>(invites);

                return Ok(invitesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка инвайтов");
                return StatusCode(500, "Произошла ошибка при обработке запроса");
            }
        }
    }
}