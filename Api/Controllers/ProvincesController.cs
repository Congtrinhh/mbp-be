using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.Paging;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;

namespace Api.Controllers
{
    public class ProvincesController : BaseController<Province, Province, PagedRequest>
    {
        public ProvincesController(IBaseService<Province, Province> baseService) : base(baseService)
        {
        }
    }
}
