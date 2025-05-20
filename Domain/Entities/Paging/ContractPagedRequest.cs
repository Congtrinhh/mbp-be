using Domain.Enums;

namespace Domain.Entities.Paging
{
    public class ContractPagedRequest : PagedRequest
    {
        public int? ClientId { get; set; }
        public int? McId { get; set; }
        public string? KeyWord { get; set; }
        public ContractStatus? Status { get; set; }
    }
}
