using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.Paging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using WebAPI.Controllers;

namespace Api.Controllers
{
    public class HostingStylesController : BaseController<HostingStyle, HostingStyle, PagedRequest>
    {
        public HostingStylesController(IBaseService<HostingStyle, HostingStyle> baseService) : base(baseService)
        {
        }
    }
}
