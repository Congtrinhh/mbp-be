using Application.Dtos.User;
using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.Paging;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;

namespace Api.Controllers
{
    public class ClientReviewMcsController : BaseController<ClientReviewMc, ClientReviewMcDto, ClientReviewMcPagedRequest>
    {
        public ClientReviewMcsController(IClientReviewMcService baseService) : base(baseService)
        {
        }
    }
}
