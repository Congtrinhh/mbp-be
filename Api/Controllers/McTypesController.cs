using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.Paging;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;

namespace Api.Controllers
{
    public class McTypesController : BaseController<McType, McType, PagedRequest>
    {
        public McTypesController(IBaseService<McType, McType> baseService) : base(baseService)
        {
        }
    }
}
