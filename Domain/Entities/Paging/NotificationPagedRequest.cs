namespace Domain.Entities.Paging
{
    public class NotificationPagedRequest: PagedRequest
    {
        public int? UserId { get; set; }
    }
}