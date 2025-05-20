using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("notification")]
    public class Notification:BaseEntity
    {
        /// <summary>
        /// receiver
        /// </summary>
        public int UserId { get; set; }
        public NotificationType Type { get; set; }
        public string? Message { get; set; }
        public bool IsRead { get; set; }
        public string? AdditionalInfo { get; set; }
        public string? ThumbUrl { get; set; }
        public NotificationStatus? Status { get; set; }
    }
}
