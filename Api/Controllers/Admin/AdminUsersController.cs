using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.Entities.Paging;
using Application.Dtos.Admin;
using Api.Controllers.Admin;
using Application.Interfaces.Admin;

namespace Application.Admin.Controllers
{
    public class AdminUsersController : AdminBaseController<User, AdminUserDto>
    {
        public AdminUsersController(IAdminUserService mcService) 
            : base(mcService)
        {
        }

    }
}