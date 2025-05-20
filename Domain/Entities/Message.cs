namespace Domain.Entities
{
    /// <summary>
    /// a message between client and MC
    /// </summary>
    public class Message : BaseEntity
    {
        public int McId { get; set; }
        public int ClientId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public string MediaUrl { get; set; } = string.Empty;
        public MediaType MediaType { get; set; }
    }
}
