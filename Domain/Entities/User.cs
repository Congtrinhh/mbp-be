using Domain.Attributes;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// một người dùng trong hệ thống:
    /// nếu IsMC = true thì vừa là MC vừa là khách tìm MC
    /// nếu IsMC = false thì là khách tìm MC
    /// </summary>
    [Table("user")]
    public class User : BaseEntity
    {
        /// <summary>
        /// họ tên - lấy từ tk google 
        /// </summary>
        public string FullName { get; set; } = string.Empty;
        /// <summary>
        /// email - lấy từ tk google 
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// số điện thoại - lấy từ tk google 
        /// </summary>
        public string? PhoneNumber { get; set; }
        public bool IsMc { get; set; }
        public int? Age { get; set; }
        /// <summary>
        /// nghệ danh
        /// </summary>
        public string? NickName { get; set; }
        /// <summary>
        /// điểm đánh giá trung bình
        /// </summary>
        public decimal? AvgRating { get; set; }

        /// <summary>
        /// số lượt đánh giá
        /// </summary>
        public int? ReviewCount { get; set; }

        public Gender Gender { get; set; }
        /// <summary>
        /// có phải MC mới không
        /// </summary>
        public bool IsNewbie { get; set; }
        /// <summary>
        /// khu vực hoạt động
        /// </summary>
        public string? WorkingArea { get; set; }
        /// <summary>
        /// đã xác thực danh tính chưa
        /// </summary>
        public bool IsVerified { get; set; }
        public string? Description { get; set; }
        public string? Education { get; set; }
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Facebook { get; set; }
        public string? Zalo { get; set; }
    }
}
    