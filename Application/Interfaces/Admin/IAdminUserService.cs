using Application.Dtos.Admin;
using Domain.Entities;
using Domain.Entities.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Admin
{
    public interface IAdminUserService : IAdminBaseService<User, AdminUserDto>
    {
        
    }
}
