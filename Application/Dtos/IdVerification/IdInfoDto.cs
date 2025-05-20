using Application.Dtos.User;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.IdVerification
{
    [Table("id_info")]
    public class IdInfoDto : BaseEntity
    {
        public int UserId { get; set; }

        public string? IdNumber { get; set; }

        public string? Name { get; set; }

        public DateTime? Dob { get; set; }

        public string? Sex { get; set; }

        public string? Nationality { get; set; }

        public string? Home { get; set; }

        public string? Address { get; set; }

        public DateTime? Doe { get; set; }

        public string? Religion { get; set; }

        public string? Ethnicity { get; set; }

        public string? Features { get; set; }

        public DateTime? IssueDate { get; set; }

        public string? IssueLoc { get; set; }

        [NotMapped]
        public UserDto? User { get; set; }
    }
}
