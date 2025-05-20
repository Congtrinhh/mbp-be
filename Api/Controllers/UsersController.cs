using Api.Attributes;
using Application.Dtos.User;
using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.Paging;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;

namespace Api.Controllers
{
    public class UsersController: BaseController<User, UserDto, UserPagedRequest>
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService) : base(userService)
        {
            _userService = userService;
        }

        [RequiresSameUser]
        [HttpPost("{userId}/avatar")]
        public async Task<IActionResult> UploadAvatar([FromRoute] int userId, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is empty");
            }

            using var stream = file.OpenReadStream();
            var user = await _userService.UploadAvatarAsync(userId, stream, file.FileName, file.ContentType);

            return Ok(user);
        }

        [RequiresSameUser]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }

        [RequiresSameUser]
        public override async Task<IActionResult> Update(int id, [FromBody] UserDto entity)
        {
            return await base.Update(id, entity);
        }
    }
}
