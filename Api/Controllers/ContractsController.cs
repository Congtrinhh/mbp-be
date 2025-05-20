using Application.Dtos.User;
using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.Paging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;

namespace Api.Controllers
{
    public class ContractsController : BaseController<Contract, ContractDto, ContractPagedRequest>
    {
        public ContractsController(IContractService baseService) : base(baseService)
        {
        }
    }
}
