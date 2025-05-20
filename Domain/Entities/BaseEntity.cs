using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Base entity class for all entities
    /// </summary>
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        [NotMapped]
        public EntityState EntityState { get; set; } = EntityState.None;
    }
}
