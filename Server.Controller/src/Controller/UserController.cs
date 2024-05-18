using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Core.src.Common;
using Server.Service.src.DTO;
using Server.Service.src.ServiceAbstract.EntityServiceAbstract;


namespace Server.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;


        public UserController(IUserService userService)
        {
            _userService = userService;

        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync([FromQuery] QueryOptions options)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                throw new InvalidOperationException("Please login to use this facility!");
            }

            var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userClaims == null)
            {
                throw new InvalidOperationException("Invalid user claims.");
            }
            return await _userService.GetAllUsersAsync(options);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<UserReadDto> GetUserByIdAsync([FromRoute] Guid id)
        {
            var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
            return await _userService.GetUserByIdAsync(id);
        }
        [Authorize]
        [HttpGet("profile")]
        public async Task<UserReadDto> GetUserProfileAsync()
        {
            var claims = HttpContext.User;
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);
            return await _userService.GetUserByIdAsync(userId);
        }

        [HttpPost]
        public async Task<UserReadDto> CreateCustomerAsync([FromBody] UserCreateDto user)
        {
            return await _userService.CreateCustomerAsync(user);
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<bool> DeleteUserByIdAsync([FromRoute] Guid id)
        {
            var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
            return await _userService.DeleteUserByIdAsync(id);
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<UserReadDto> UpdateUserByIdAsync([FromRoute] Guid id, [FromBody] UserUpdateDto userUpdateDto)
        {
            var userClaims = (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new InvalidOperationException("Please login to use this facility!");
            return await _userService.UpdateUserByIdAsync(id, userUpdateDto);
        }
        [Authorize]
        [HttpPatch("change_password")]
        public async Task<bool> ChangePassword([FromBody] string newPassword)
        {
            var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
            var userId = Guid.Parse(userClaims);
            return await _userService.ChangePasswordAsync(userId, newPassword);
        }

        [HttpPost("upload-avatar"), Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadAvatar([FromForm] UserForm userForm)
        {
            var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = Guid.Parse(userClaims);
            if (userForm.AvatarImage == null || userForm.AvatarImage.Length == 0)
            {
                return BadRequest("Avatar is missing");
            }
            else
            {
                using (var ms = new MemoryStream())
                {
                    await userForm.AvatarImage.CopyToAsync(ms);
                    var content = ms.ToArray();
                    var uploaded = await _userService.UploadAvatarAsync(userId, content);
                    Console.WriteLine(uploaded);
                    if(uploaded)
                        return File(content, userForm.AvatarImage.ContentType);
                    throw new DbUpdateException("Failed to upload avatar...");
                }
            }
        }
    }

    public class UserForm
    {
        public IFormFile AvatarImage { get; set; }
        public Guid UserId { get; set; }
    }
}