using Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("user_id_verification")]
    public class UserIdVerification : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        public string FaceImageUrl { get; set; } = string.Empty;

        public string IdFrontImageUrl { get; set; } = string.Empty;

        public string IdBackImageUrl { get; set; } = string.Empty;

        public VerificationStep CurrentStep { get; set; }

        public VerificationStatus Status { get; set; }

        public DateTime? VerifiedAt { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [NotMapped]
        public IdInfo IdInfo { get; set; }
    }
}