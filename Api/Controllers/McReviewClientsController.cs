using Application.Dtos.User;
using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.Paging;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;

namespace Api.Controllers
{
    public class McReviewClientsController : BaseController<McReviewClient, McReviewClientDto, McReviewClientPagedRequest>
    {
        public McReviewClientsController(IMcReviewClientService baseService) : base(baseService)
        {
        }
    }
}
