using Domain.Attributes;
using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Dtos.User
{
    [Table("client_review_mc")]
    public class ClientReviewMcDto : BaseEntity
    {
        public int? ContractId { get; set; }
        public int? ClientId { get; set; }
        public int? McId { get; set; }
        public int? ProPoint { get; set; }
        public int? AttitudePoint { get; set; }
        public int? OverallPoint { get; set; }
        public int? ReliablePoint { get; set; }
        public string? ShortDescription { get; set; }
        public string? DetailDescription { get; set; }


        [OneToOne]
        public Contract? Contract { get; set; }
        [OneToOne]
        public Domain.Entities.User? Mc { get; set; }
        [OneToOne]
        public Domain.Entities.User? Client { get; set; }
    }
}
