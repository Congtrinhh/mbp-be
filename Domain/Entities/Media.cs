namespace Domain.Entities
{
    /// <summary>
    /// video/photo of MC
    /// </summary>
    public class Media : BaseEntity
    {
        public int UserId { get; set; }
        public MediaType Type { get; set; }
        public string Url { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }
}
