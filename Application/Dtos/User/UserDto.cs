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
    [Table("user")]
    public class UserDto : BaseEntity
    {
        /// <summary>
        /// họ tên - lấy từ tk google 
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// email - lấy từ tk google 
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// số điện thoại - lấy từ tk google 
        /// </summary>
        public string? PhoneNumber { get; set; }

        public bool? IsMc { get; set; }
        public int? Age { get; set; }

        /// <summary>
        /// nghệ danh
        /// </summary>
        public string? NickName { get; set; }

        /// <summary>
        /// điểm đánh giá trung bình
        /// </summary>
        [NotMapped]
        public decimal? AvgRating { get; set; }

        /// <summary>
        /// số lượt đánh giá
        /// </summary>
        [NotMapped]
        public int? ReviewCount { get; set; }

        public Gender? Gender { get; set; }

        /// <summary>
        /// có phải MC mới không
        /// </summary>
        public bool? IsNewbie { get; set; }

        /// <summary>
        /// khu vực hoạt động
        /// </summary>
        public string? WorkingArea { get; set; }

        /// <summary>
        /// đã xác thực danh tính chưa
        /// </summary>
        public bool? IsVerified { get; set; }

        public string? Description { get; set; }
        public string? Education { get; set; }
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Facebook { get; set; }
        public string? Zalo { get; set; }

        [ChildTable("user_id")]
        [NotMapped]
        [OneToMany(table: "media", foreignKey: "user_id")]
        public IList<Media>? Medias { get; set; }

        [NotMapped]
        [ManyToMany(joinTable: "mc_mc_type", joinColumn: "mc_id", inverseJoinColumn: "mc_type_id")]
        public IList<McType>? McTypes { get; set; }

        [NotMapped]
        [ManyToMany(joinTable: "mc_province", joinColumn: "mc_id", inverseJoinColumn: "province_id")]
        public IList<Province>? Provinces { get; set; }

        [NotMapped]
        [ManyToMany(joinTable: "mc_hosting_style", joinColumn: "mc_id", inverseJoinColumn: "hosting_style_id")]
        public List<HostingStyle>? HostingStyles { get; set; }
    }
}
