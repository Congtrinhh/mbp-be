using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.Dtos.IdVerification;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/identity-verification")]
    public class UserIdVerificationController : ControllerBase
    {
        private readonly IUserIdVerificationService _idVerificationService;

        public UserIdVerificationController(IUserIdVerificationService idVerificationService)
        {
            _idVerificationService = idVerificationService;
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetStatus()
        {
            // TODO: Get user ID from authenticated user
            var userId = 0; // Temporary hardcoded value
            var status = await _idVerificationService.GetVerificationStatusAsync(userId);
            return Ok(status);
        }

        [HttpPost("face")]
        public async Task<IActionResult> UploadFacePhoto([FromBody] FacePhotoUploadDto photo)
        {
            if (string.IsNullOrEmpty(photo?.ImageBase64))
            {
                return BadRequest("No photo provided");
            }

            // TODO: Get user ID from authenticated user
            var userId = 0; // Temporary hardcoded value
            var result = await _idVerificationService.UploadFacePhotoAsync(userId, photo);
            return Ok(result);
        }

        [HttpPost("id-card")]
        public async Task<IActionResult> UploadIdCardPhoto([FromBody] IdCardPhotoUploadDto photo)
        {
            if (string.IsNullOrEmpty(photo?.ImageBase64) || string.IsNullOrEmpty(photo?.Side))
            {
                return BadRequest("Invalid photo data");
            }

            if (photo.Side != "front" && photo.Side != "back")
            {
                return BadRequest("Invalid card side specified");
            }

            // TODO: Get user ID from authenticated user
            var userId = 0; // Temporary hardcoded value
            var result = await _idVerificationService.UploadIdCardPhotoAsync(userId, photo);
            return Ok(result);
        }

        [HttpGet("info")]
        public async Task<IActionResult> GetIdInfo()
        {
            // TODO: Get user ID from authenticated user
            var userId = 0; // Temporary hardcoded value
            var info = await _idVerificationService.GetIdInfoAsync(userId);
            return Ok(info);
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmInfo([FromBody] IdInfoDto info)
        {
            if (info == null)
            {
                return BadRequest("No information provided");
            }

            // TODO: Get user ID from authenticated user
            var userId = 0; // Temporary hardcoded value
            var result = await _idVerificationService.ConfirmIdInfoAsync(userId, info);
            
            if (result)
            {
                return Ok(new { message = "Identity verification completed successfully" });
            }
            
            return BadRequest("Failed to confirm identity information");
        }
    }
}