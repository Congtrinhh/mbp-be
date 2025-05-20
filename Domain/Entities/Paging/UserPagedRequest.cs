using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Paging
{
    public class UserPagedRequest : PagedRequest
    {
        public string? FullName { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public bool? IsMc { get; set; }
        public bool? IsVerified { get; set; }
        public bool? IsNewbie { get; set; }
        public string? NickName { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }

        #region Get details
        public bool? IsGetMcType { get; set; } = false;
        public bool? IsGetHostingStyle { get; set; } = false;
        public bool? IsGetMedia { get; set; } = false;
        public bool? IsGetProvince { get; set; }
        #endregion 

        // New properties based on the provided JSON
        public string? McTypeIds { get; set; } = string.Empty;
        public string? HostingStyleIds { get; set; } = string.Empty;
        public string? ProvinceIds { get; set; } = string.Empty;
        public string? Genders { get; set; } = string.Empty;
    }
}
