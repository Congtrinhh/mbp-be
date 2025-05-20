using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.User
{
    [Table("post")]
    public class PostDto: BaseEntity
    {
        /// <summary>
        /// Foreign key to the client.
        /// </summary>
        public int? UserId { get; set; }
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

        /// <summary>
        /// User who created the post.
        /// </summary>
        [NotMapped]
        public Domain.Entities.User? User { get; set; } 
        [NotMapped]
        public IList<Reaction>? Reactions { get; set; } = new List<Reaction>();
    }
}
