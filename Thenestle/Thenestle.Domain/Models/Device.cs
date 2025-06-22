using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thenestle.Domain.Models
{
    [Table("device", Schema = "couple_app")]
    public class Device
    {
        [Column("device_id")]
        public int DeviceId { get; set; }

        [Column("device_type")]
        public string DeviceType { get; set; } // iOS/Android/Web/Telegram

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("last_active_at")]
        public DateTime LastActiveAt { get; set; } = DateTime.UtcNow;

        [Column("user_id")]
        public int UserId { get; set; }

        // Навигационные свойства
        public Users User { get; set; }
    }
}
