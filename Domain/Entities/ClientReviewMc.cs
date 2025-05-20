using Domain.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Entity representing a client review of an MC.
    /// </summary>
    [Table("client_review_mc")]
    public class ClientReviewMc : BaseEntity
    {
        /// <summary>
        /// Foreign key to the contract.
        /// </summary>
        public int? ContractId { get; set; }

        /// <summary>
        /// Foreign key to the client.
        /// </summary>
        public int? ClientId { get; set; }

        /// <summary>
        /// Foreign key to the MC.
        /// </summary>
        public int? McId { get; set; }

        /// <summary>
        /// Client's rating of the MC's professional skills (scale from 1 to 5).
        /// </summary>
        public int ProPoint { get; set; }

        /// <summary>
        /// Client's rating of the MC's work attitude (scale from 1 to 5).
        /// </summary>
        public int AttitudePoint { get; set; }

        /// <summary>
        /// Client's rating of the MC's overall performance (scale from 1 to 5).
        /// </summary>
        public int OverallPoint { get; set; }

        /// <summary>
        /// Client's rating of the MC's reliability (scale from 1 to 5).
        /// </summary>
        public int ReliablePoint { get; set; }

        /// <summary>
        /// Title of the post.
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Content of the post.
        /// </summary>
        public string DetailDescription { get; set; }
    }
}
