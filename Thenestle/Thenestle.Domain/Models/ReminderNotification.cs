using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thenestle.Domain.Models
{
    [Table("reminder_notification", Schema = "couple_app")]
    public class ReminderNotification
    {
        [Column("notification_id")]
        public int NotificationId { get; set; }

        [Column("days_before")]
        public int DaysBefore { get; set; } // 1, 3, 7

        [Column("is_sent")]
        public bool IsSent { get; set; } = false;

        [Column("sent_at")]
        public DateTime? SentAt { get; set; }

        [Column("reminder_id")]
        public int ReminderId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        // Навигационные свойства
        public Reminder Reminder { get; set; }
        public Users User { get; set; }
    }
}
