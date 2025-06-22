using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thenestle.API.DTO.Couples;
using Thenestle.API.DTO.Invites;
using Thenestle.Domain.Interfaces.Repositories;
using Thenestle.Domain.Interfaces.Services;
using Thenestle.Domain.Models;

namespace Thenestle.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/invites")]
    [Authorize]
    public class InvitesController : ControllerBase
    {
        private readonly IInviteRepository _inviteRepository;
        private readonly ICoupleRepository _coupleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<InvitesController> _logger;
        private readonly IGeneratorCode _generatorCode;

        public InvitesController(
            IInviteRepository inviteRepository,
            ICoupleRepository coupleRepository,
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<InvitesController> logger,
            IGeneratorCode generatorCode)
        {
            _inviteRepository = inviteRepository;
            _coupleRepository = coupleRepository;
            _userRepository = userRepository;
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

        [HttpPost("generate/{coupleId}")]
        public async Task<ActionResult<InviteDTO>> GenerateInvite(int coupleId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var couple = await _coupleRepository.GetCoupleByIdAsync(coupleId);

                if (couple == null)
                {
                    return NotFound("Couple not found");
                }

                if (couple.User1Id != userId && couple.User2Id != userId)
                {
                    return Forbid("You don't have permission for this couple");
                }

                // Generate new invite
                var invite = new Invite
                {
                    CoupleId = coupleId,
                    InviterId = userId,
                    Code = _generatorCode.GenerateCodeAsync(),
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    IsUsed = false,
                    Status = "active"
                };

                await _inviteRepository.AddInviteAsync(invite);

                return Ok(_mapper.Map<InviteDTO>(invite));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating invite");
                return StatusCode(500, "Error generating invite");
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

        [HttpPost("accept")]
        public async Task<ActionResult<CoupleDTO>> AcceptInvite([FromBody] string code)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                // Check if user already has a couple
                var existingCouple = await _coupleRepository.GetCoupleByUserIdAsync(userId);
                if (existingCouple != null)
                {
                    return BadRequest("You already have a couple");
                }

                // Find active invite by code
                var invite = await _inviteRepository.GetInviteByCodeAsync(code);
                if (invite == null || invite.IsUsed || invite.ExpiresAt < DateTime.UtcNow)
                {
                    return BadRequest("Invalid or expired invite code");
                }

                // Get the couple
                var couple = await _coupleRepository.GetCoupleByIdAsync(invite.CoupleId);
                if (couple == null)
                {
                    return NotFound("Couple not found");
                }

                // Complete the couple by adding the second user
                couple.User2Id = userId;
                await _coupleRepository.UpdateCoupleAsync(couple);

                // Update user's couple reference
                var user = await _userRepository.GetUserByIdAsync(userId);
                user.CoupleId = couple.CoupleId;
                await _userRepository.UpdateUserAsync(user);

                // Mark invite as used
                invite.IsUsed = true;
                invite.Status = "accepted";
                await _inviteRepository.UpdateInviteAsync(invite);

                return Ok(_mapper.Map<CoupleDTO>(couple));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error accepting invite");
                return StatusCode(500, "Error accepting invite");
            }
        }

        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<InviteDTO>>> GetMyInvites()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized("Пользователь не аутентифицирован");

                int userId = int.Parse(userIdClaim.Value);

                var invites = await _inviteRepository.GetInvitesByUserIdAsync(userId);
                return Ok(_mapper.Map<IEnumerable<InviteDTO>>(invites));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка инвайтов пользователя");
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