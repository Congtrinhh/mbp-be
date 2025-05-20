using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Paging;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<T, TDto, TRequest> : ControllerBase where T : BaseEntity where TDto : BaseEntity where TRequest : PagedRequest
    {
        protected readonly IBaseService<T, TDto> BaseService;

        public BaseController(IBaseService<T, TDto> baseService)
        {
            BaseService = baseService;
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(int id)
        {
            var entity = await BaseService.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            var entities = await BaseService.GetAllAsync();
            return Ok(entities);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Add([FromBody] TDto entity)
        {
            if (entity == null)
            {
                return BadRequest();
            }

            var result = await BaseService.AddAsync(entity);
            if (result ==0)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return CreatedAtAction(nameof(GetById), new { id = entity.GetType().GetProperty("Id")?.GetValue(entity) }, entity);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(int id, [FromBody] TDto entity)
        {
            if (entity == null || id != (int)entity.GetType().GetProperty("Id")?.GetValue(entity))
            {
                return BadRequest();
            }

            var result = await BaseService.UpdateAsync(entity);
            if (result == 0)

            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            var result = await BaseService.DeleteAsync(id);
            if (result == 0)

            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [HttpPost("paged")]
        public async Task<IActionResult> GetPaged([FromBody] TRequest pagedRequest)
        {
            var result = await BaseService.GetPagedAsync(pagedRequest);
            return Ok(result);
        }
    }
}
