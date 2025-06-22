namespace Thenestle.API.DTO.Couples
{
    public class CoupleDTO
    {
        public int CoupleId { get; set; }
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
