using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Entities
{
    [Table("reaction")]
    public class Reaction:BaseEntity
    {
        [Required]
        public int PostId { get; set; }

        public int? UserId { get; set; }
        /// <summary>
        /// nick name của MC / user name của khách tìm MC
        /// </summary>

        [MaxLength(255)]
        public string? UserName { get; set; }

        [Required]
        public ReactionType Type { get; set; } = ReactionType.Like;
    }
}
