using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thenestle.Domain.Models
{
    [Table("place", Schema = "couple_app")]
    public class Place
    {
        [Column("place_id")]
        public int PlaceId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("latitude")]
        public decimal Latitude { get; set; }

        [Column("longitude")]
        public decimal Longitude { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("category")]
        public string? Category { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("added_by_id")]
        public int AddedById { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Навигационные свойства
        public Users AddedBy { get; set; }
    }
}
