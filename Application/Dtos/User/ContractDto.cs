using Domain.Attributes;
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
    [Table("contract")]
    public class ContractDto : BaseEntity
    {
        public int? McId { get; set; }
        public int? ClientId { get; set; }
        public DateTime? EventStart { get; set; } // thời gian MC bắt đầu dẫn chương trình (bắt đầu ca dẫn)
        public DateTime? EventEnd { get; set; } // thời gian MC kết thúc dẫn chương trình (xong việc)
        public string? Description { get; set; }  // Mô tả về chương trình
        public string? Place { get; set; }  // địa điểm sự kiện diễn ra
        public DateTime? McCancelDate { get; set; } // thời gian MC hủy hợp đồng. nếu trường này có giá trị tức là hợp đồng này bị hủy
        public string? McCancelReason { get; set; } // lý do MC hủy hợp đồng
        public DateTime? ClientCancelDate { get; set; } // thời gian khách book MC hủy hợp đồng. nếu trường này có giá trị tức là hợp đồng này bị hủy
        public string? ClientCancelReason { get; set; } // lý do khách book MC hủy hợp đồng
        public string? EventName { get; set; }
        public ContractStatus? Status { get; set; }
        /// <summary>
        /// Đã gửi nhắc nhở đánh giá hay chưa
        /// </summary>
        public bool IsRemind { get; set; }

        [NotMapped]
        public Domain.Entities.User? Client { get; set; }

        [NotMapped]
        public Domain.Entities.User? Mc { get; set; }

        /// <summary>
        /// Flag to indicate if buffer time check should be ignored
        /// Used when user explicitly accepts the buffer time warning
        /// </summary>
        [NotMapped]
        public bool? IsIgnoreBufferCheck { get; set; }
    }
}
