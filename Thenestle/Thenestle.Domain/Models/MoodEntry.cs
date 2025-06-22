using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thenestle.Domain.Models
{
    [Table("mood_entry", Schema = "couple_app")]
    public class MoodEntry
    {
        [Column("mood_entry_id")]
        public int MoodEntryId { get; set; }

        [Column("mood_id")]
        public int MoodId { get; set; }

        [Column("comment")]
        public string? Comment { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("user_id")]
        public int UserId { get; set; }

        // Навигационные свойства
        public Mood Mood { get; set; }
        public User User { get; set; }
    }
}
