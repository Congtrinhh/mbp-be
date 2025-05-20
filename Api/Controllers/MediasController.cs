using Application.Interfaces;
using Domain.Entities.Paging;
using Domain.Entities;
using WebAPI.Controllers;
using Application.Dtos.User;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class MediasController : BaseController<Media, MediaDto, MediaPagedRequest>
    {
        public MediasController(IMediaService baseService) : base(baseService)
        {
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] int userId, [FromForm] MediaType type, [FromForm] int sortOrder)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var mediaDto = new MediaDto
            {
                UserId = userId,
                Type = type,
                SortOrder = sortOrder,
                File = file
            };

            var result = await BaseService.AddAsync(mediaDto);
            if (result == 0)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }
    }
}
