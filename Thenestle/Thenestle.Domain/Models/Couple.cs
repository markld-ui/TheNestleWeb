using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thenestle.Domain.Models
{
    [Table("couple", Schema = "couple_app")]
    public class Couple
    {
        [Column("couple_id")]
        public int CoupleId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("user1_id")]
        public int User1Id { get; set; }

        [Column("user2_id")]
        public int User2Id { get; set; }

        // Навигационные свойства
        public Users User1 { get; set; }
        public Users User2 { get; set; }
        public ICollection<Invite> Invites { get; set; }
    }
}
