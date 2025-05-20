namespace Application.Dtos.Notification
{
    public class SendOfferAdditionalInfo
    {
        public string EventName { get; set; }
        public DateTime EventStart { get; set; }
        public DateTime EventEnd { get; set; }
        public string Place { get; set; }
        public string Note { get; set; }
        public int? SenderId { get; set; }
        public string SenderName { get; set; }
    }
}
