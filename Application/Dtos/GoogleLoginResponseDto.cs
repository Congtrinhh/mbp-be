using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class GoogleLoginResponseDto
    {
        /// <summary>
        /// token chứa đầy đủ thông (tức là đã login thành công)
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;
        /// <summary>
        /// nếu = true, client cần set IsMc và gửi lại request để tạo mới 
        /// </summary>
        public bool IsNewUser { get; set; } = false;
    }
}
