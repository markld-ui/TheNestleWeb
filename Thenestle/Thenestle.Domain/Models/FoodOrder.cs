using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thenestle.Domain.Models
{
    [Table("food_order", Schema = "couple_app")]
    public class FoodOrder
    {
        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("price")]
        public int Price { get; set; }

        [Column("status")]
        public string Status { get; set; } = "pending"; // pending/confirmed/canceled/delivered

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("approved_at")]
        public DateTime? ApprovedAt { get; set; }

        [Column("initiator_id")]
        public int InitiatorId { get; set; }

        // Навигационные свойства
        public Users Initiator { get; set; }
        public CurrencyTransaction? Transaction { get; set; }
    }
}
