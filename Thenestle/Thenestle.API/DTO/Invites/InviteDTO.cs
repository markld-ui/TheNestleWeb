namespace Thenestle.API.DTO.Invites
{
    public class InviteDTO
    {
        public int InviteId { get; set; }
        public int InviterId { get; set; }
        public int CoupleId { get; set; }
        public string Code { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }
        public string Status { get; set; }
    }
}
