using Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Entity representing a post for finding an MC for an event.
    /// </summary>
    [Table("post")]
    public class Post : BaseEntity
    {
        /// <summary>
        /// Foreign key to the client.
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// caption of the post
        /// </summary>
        public string? Caption { get; set; }
        /// <summary>
        /// Group of the post.
        /// </summary>
        public PostGroup? PostGroup { get; set; }

        /// <summary>
        /// Location of the event.
        /// </summary>
        public string? Place { get; set; }

        /// <summary>
        /// Description of the event, including the name of the event.
        /// </summary>
        public string? EventName { get; set; }

        /// <summary>
        /// Start time of the event.
        /// </summary>
        public DateTime? EventStart { get; set; }

        /// <summary>
        /// End time of the event.
        /// </summary>
        public DateTime? EventEnd { get; set; }

        /// <summary>
        /// Minimum fee for the MC.
        /// </summary>
        public decimal? PriceFrom { get; set; }

        /// <summary>
        /// Maximum fee for the MC.
        /// </summary>
        public decimal? PriceTo { get; set; }

        /// <summary>
        /// Description of the requirements for the MC.
        /// </summary>
        public string? McRequirement { get; set; }
        /// <summary>
        /// detailed description about the event
        /// </summary>
        public string? DetailDesc { get; set; }
    }
}
