using System;
using Domain.Entities;

namespace Application.Dtos.Admin
{
    /// <summary>
    /// Base DTO for admin operations that includes audit fields
    /// Created by: tqcong 27/04/2025
    /// </summary>
    public class AdminBaseDto : BaseEntity
    {
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
    }

    /// <summary>
    /// DTO for admin user operations
    /// Created by: tqcong 27/04/2025
    /// </summary>
    public class AdminUserDto : AdminBaseDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsMc { get; set; }
        public int? Age { get; set; }
        public string NickName { get; set; } = string.Empty;
        public decimal Credit { get; set; }
        public int Gender { get; set; }
        public bool IsNewbie { get; set; }
        public string WorkingArea { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Education { get; set; } = string.Empty;
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public string AvatarUrl { get; set; } = string.Empty;
        public string Facebook { get; set; } = string.Empty;
        public string Zalo { get; set; } = string.Empty;
    }
}