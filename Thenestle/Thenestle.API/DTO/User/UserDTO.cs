using Thenestle.Domain.Models;

namespace Thenestle.API.DTO.User
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Gender { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CurrencyBalance { get; set; }
        public int? CoupleId { get; set; }
    }
}
