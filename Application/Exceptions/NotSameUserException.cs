using Application.Dtos.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    /// <summary>
    /// Khi ta yêu cầu một hành động phải là của chính mình
    /// ví dụ: xóa bài viết của chính mình thì được, 
    /// nhưng nếu xóa bài viết của người khác thì không được => sẽ throw NotSameUserException
    /// </summary>
    public class NotSameUserException : Exception
    {

    }
}
