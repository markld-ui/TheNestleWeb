using System.ComponentModel.DataAnnotations;

namespace Thenestle.API.DTO.User
{
    public class UpdateUserDTO
    {
        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Gender { get; set; }
    }
}
