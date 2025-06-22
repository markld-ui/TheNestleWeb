using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thenestle.Domain.Models
{
    [Table("currency_transaction", Schema = "couple_app")]
    public class CurrencyTransaction
    {
        [Column("transaction_id")]
        public int TransactionId { get; set; }

        [Column("amount")]
        public int Amount { get; set; }

        [Column("type")]
        public string Type { get; set; } // order/bonus/correction/refund

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("order_id")]
        public int? OrderId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Навигационные свойства
        public User User { get; set; }
        public FoodOrder? Order { get; set; }
    }
}
