using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thenestle.Domain.Models
{
    [Table("reminder", Schema = "couple_app")]
    public class Reminder
    {
        [Column("reminder_id")]
        public int ReminderId { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("remind_at")]
        public DateTime RemindAt { get; set; }

        [Column("type")]
        public string Type { get; set; } // personal/shared

        [Column("status")]
        public string Status { get; set; } = "active"; // active/completed/canceled

        [Column("created_by_id")]
        public int CreatedById { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Навигационные свойства
        public Users CreatedBy { get; set; }
        public ICollection<ReminderNotification> Notifications { get; set; }
    }
}
