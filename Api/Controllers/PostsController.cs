using Api.Attributes;
using Application.Dtos.User;
using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.Paging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;

namespace Api.Controllers
{
    public class PostsController : BaseController<Post, PostDto, PostPagedRequest>
    {
        public PostsController(IPostService baseService) : base(baseService)
        {
        }

        [RequiresSameUser]
        public async override Task<IActionResult> Update(int id, [FromBody] PostDto entity)
        {
            return await base.Update(id, entity);
        }

        public async override Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }

        [RequiresSameUser]
        public async override Task<IActionResult> Add([FromBody] PostDto entity)
        {
            return await base.Add(entity);
        }
    }
}
