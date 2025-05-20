//using System;
//using System.ComponentModel.DataAnnotations.Schema;
//using Application.Dtos.User;
//using Domain.Entities;

//namespace Application.Dtos.IdVerification
//{
//    [Table("user_id_verification")]
//    public class IdVerificationDto : BaseEntity
//    {
//        public int? UserId { get; set; }
//        public string FaceImageUrl { get; set; }
//        public string IdFrontImageUrl { get; set; }
//        public string IdBackImageUrl { get; set; }
//        public byte CurrentStep { get; set; }
//        public byte Status { get; set; }
//        public DateTime? VerifiedAt { get; set; }
//        public string ErrorMessage { get; set; }

//        [NotMapped]
//        public virtual UserDto User { get; set; }

//        [NotMapped]
//        public virtual IdInfoDto ExtractedInfo { get; set; }
//    }
//}