using Application.Dtos.Admin;
using Application.Interfaces.Admin;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Admin
{
    public class AdminUserService : AdminBaseService<User, AdminUserDto>, IAdminUserService
    {
        public AdminUserService(IAdminBaseRepository<User> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
