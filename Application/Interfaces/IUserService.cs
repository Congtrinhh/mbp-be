using Application.Dtos.User;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService: IBaseService<User, UserDto>
    {
        Task<UserDto> UploadAvatarAsync(int userId, Stream fileStream, string fileName, string contentType);
    }
}
