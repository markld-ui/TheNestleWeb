using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thenestle.Domain.Models
{
    [Table("invite", Schema = "couple_app")]
    public class Invite
    {
        [Column("invite_id")]
        public int InviteId { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("is_used")]
        public bool IsUsed { get; set; } = false;

        [Column("status")]
        public string Status { get; set; } = "pending"; // pending/accepted/rejected/expired

        [Column("expires_at")]
        public DateTime ExpiresAt { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("couple_id")]
        public int CoupleId { get; set; }

        [Column("inviter_id")]
        public int InviterId { get; set; }

        // Навигационные свойства
        public Couple Couple { get; set; }
        public Users Inviter { get; set; }
    }
}
