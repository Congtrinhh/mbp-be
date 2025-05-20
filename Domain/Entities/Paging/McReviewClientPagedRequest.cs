namespace Domain.Entities.Paging
{
    public class McReviewClientPagedRequest : PagedRequest
    {
        public int? ClientId { get; set; }
        public bool? IsGetContract { get; set; } = false;
        public bool? IsGetMc { get; set; } = false;
        public bool? IsGetClient { get; set; } = false;
    }
}
