using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thenestle.Domain.Models
{
    [Table("mood", Schema = "couple_app")]
    public class Mood
    {
        [Column("mood_id")]
        public int MoodId { get; set; }

        [Column("emoji")]
        public string Emoji { get; set; }

        [Column("name")]
        public string Name { get; set; }

        // Навигационное свойство
        public ICollection<MoodEntry> MoodEntries { get; set; }
    }
}
