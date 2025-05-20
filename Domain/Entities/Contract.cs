using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities
{
    /// <summary>
    /// A contract between a client and a MC
    /// </summary>
    [Table("contract")]
    public class Contract : BaseEntity
    {
        public int McId { get; set; }
        public int ClientId { get; set; }
        public string EventName { get; set; }
        public DateTime EventStart { get; set; } // thời gian MC bắt đầu dẫn chương trình (bắt đầu ca dẫn)
        public DateTime EventEnd { get; set; } // thời gian MC kết thúc dẫn chương trình (xong việc)
        public string Place { get; set; }  // địa điểm sự kiện diễn ra
        public string? Description { get; set; }  // Mô tả về chương trình
        public DateTime? McCancelDate { get; set; } // thời gian MC hủy hợp đồng. nếu trường này có giá trị tức là hợp đồng này bị hủy
        public string? McCancelReason { get; set; } // lý do MC hủy hợp đồng
        public DateTime? ClientCancelDate { get; set; } // thời gian khách book MC hủy hợp đồng. nếu trường này có giá trị tức là hợp đồng này bị hủy
        public string? ClientCancelReason { get; set; } // lý do khách book MC hủy hợp đồng
        public ContractStatus? Status { get; set; }
        /// <summary>
        /// Đã gửi nhắc nhở đánh giá hay chưa
        /// </summary>
        public bool? IsRemind { get; set; }
    }
}
