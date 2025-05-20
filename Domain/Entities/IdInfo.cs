using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("id_info")]
    public class IdInfo : BaseEntity
    {
        public int UserId { get; set; }

        public string IdNumber { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public DateTime? Dob { get; set; }

        public string Sex { get; set; } = string.Empty;

        public string Nationality { get; set; } = string.Empty;

        public string Home { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public DateTime? Doe { get; set; }

        public string? Religion { get; set; } = string.Empty;

        public string? Ethnicity { get; set; } = string.Empty;

        public string Features { get; set; } = string.Empty;

        public DateTime? IssueDate { get; set; }

        public string IssueLoc { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}