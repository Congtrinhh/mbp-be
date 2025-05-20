using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    /// <summary>
    /// Entity representing an MC's review of a client.
    /// </summary>
    public class McReviewClient : BaseEntity
    {
        /// <summary>
        /// Foreign key to the MC.
        /// </summary>
        public int? McId { get; set; }

        /// <summary>
        /// Foreign key to the client.
        /// </summary>
        public int? ClientId { get; set; }

        /// <summary>
        /// Foreign key to the contract.
        /// </summary>
        public int? ContractId { get; set; }

        /// <summary>
        /// MC's rating of the client's payment punctuality (scale from 1 to 5).
        /// </summary>
        public int PaymentPunctualPoint { get; set; }

        /// <summary>
        /// Title of the post.
        /// </summary>
        public string? ShortDescription { get; set; }

        /// <summary>
        /// Content of the post.
        /// </summary>
        public string? DetailDescription { get; set; }

        public int OverallPoint { get; set; }
        public int ReliablePoint { get; set; }


    }
}
