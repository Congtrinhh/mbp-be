namespace Domain.Entities.Paging
{
    public class ClientReviewMcPagedRequest : PagedRequest
    {
        public int? McId { get; set; }
        public bool? IsGetContract { get; set; } = false;
        public bool? IsGetMc { get; set; } = false;
        public bool? IsGetClient { get; set; } = false;
    }
}
