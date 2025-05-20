using System;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Dtos.User;
using Domain.Entities;
using Domain.Enums;

namespace Application.Dtos.IdVerification
{
    [Table("user_id_verification")]
    public class UserIdVerificationDto : BaseEntity
    {
        public int UserId { get; set; }

        public string? FaceImageUrl { get; set; }

        public string? IdFrontImageUrl { get; set; }

        public string? IdBackImageUrl { get; set; }

        public VerificationStep? CurrentStep { get; set; }

        public VerificationStatus? Status { get; set; }

        public DateTime? VerifiedAt { get; set; }

        public string? ErrorMessage { get; set; }

        [NotMapped]
        public virtual UserDto User { get; set; }

        [NotMapped]
        public virtual IdInfoDto IdInfo { get; set; }
    }

    public class FacePhotoUploadDto
    {
        public string ImageBase64 { get; set; }
    }

    public class IdCardPhotoUploadDto
    {
        public string ImageBase64 { get; set; }
        public string Side { get; set; } // "front" or "back"
    }
}