using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.Entities.Paging;
using Application.Interfaces.Admin;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers.Admin
{
    //[Authorize(Roles = "Admin")]
    [Route("api/admin/[controller]")]
    [ApiController]
    public abstract class AdminBaseController<TEntity, TDto> : ControllerBase
        where TEntity : BaseEntity
        where TDto : class
    {
        protected readonly IAdminBaseService<TEntity, TDto> _service;

        protected AdminBaseController(IAdminBaseService<TEntity, TDto> service)
        {
            _service = service;
        }

        [HttpPost("paged")]
        public virtual async Task<IActionResult> GetPaged([FromBody] BaseAdminPagedRequest request)
        {
            var result = await _service.GetPagedAsync(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Add([FromBody] TDto dto)
        {
            var result = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = 0 }, result);//TODO: getid
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(int id, [FromBody] TDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}