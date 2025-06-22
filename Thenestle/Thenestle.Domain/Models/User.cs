using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thenestle.Domain.Models
{
    [Table("user", Schema = "couple_app")]
    public class User
    {
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("password_hash")]
        public string PasswordHash { get; set; }

        [Column("currency_balance")]
        public int CurrencyBalance { get; set; } = 0;

        [Column("gender")]
        public string? Gender { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("refresh_token")]
        public string? RefreshToken { get; set; }

        [Column("refresh_token_expiry_time")]
        public DateTime? RefreshTokenExpiryTime { get; set; }

        [Column("couple_id")]
        public int? CoupleId { get; set; }

        // Навигационные свойства
        public Couple? Couple { get; set; }
        public ICollection<MoodEntry> MoodEntries { get; set; }
        public ICollection<Reminder> Reminders { get; set; }
        public ICollection<FoodOrder> InitiatedOrders { get; set; }
        public ICollection<Place> Places { get; set; }
        public ICollection<Device> Devices { get; set; }
        public ICollection<CurrencyTransaction> Transactions { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
