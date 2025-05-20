using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class GoogleLoginRequestDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public bool IsCreateUser { get; set; }
        public bool? IsMc { get; set; }
        public bool? IsNewbie { get; set; }
    }
}
