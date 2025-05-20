using AutoMapper.Configuration.Annotations;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Dtos.User
{
    [Table("media")]
    public class MediaDto : BaseEntity
    {
        public int? UserId { get; set; }
        public MediaType? Type { get; set; }
        public string? Url { get; set; }
        public int? SortOrder { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
    }
}
