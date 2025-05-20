using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Paging
{
    public class PostPagedRequest:PagedRequest
    {
        public PostGroup? PostGroup { get; set; }
        public string? KeyWord { get; set; }
        public bool? IsGetReaction { get; set; }
    }
}
