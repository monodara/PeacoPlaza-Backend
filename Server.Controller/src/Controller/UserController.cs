using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Core.src.Common;
using Server.Service.src.DTO;
using Server.Service.src.ServiceAbstract.EntityServiceAbstract;
using Server.Service.src.Shared;


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
            if (HttpContext.User!.Identity == null || !HttpContext.User.Identity.IsAuthenticated)
            {
                throw CustomException.UnauthorizedException("");
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
        public async Task<ActionResult<UserReadDto>> GetUserByIdAsync([FromRoute] Guid id)
        {
            var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
            var userReadDto = await _userService.GetUserByIdAsync(id);
            if (userReadDto == null)
            {
                return NotFound();
            }
            return Ok(userReadDto);
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
        public async Task<ActionResult<UserReadDto>> CreateCustomerAsync([FromBody] UserCreateDto user)
        {
            if (user == null)
            {
                return BadRequest("User data is required");
            }
            try
            {
                var createdUser = await _userService.CreateCustomerAsync(user);
                if (createdUser == null)
                {
                    return BadRequest("User could not be created");
                }
                Console.WriteLine(createdUser.Id);
                return CreatedAtAction(nameof(CreateCustomerAsync), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("check-email")]
        public async Task<bool> CheckEmailAsync([FromBody] EmailRequest request)
        {
            return await _userService.CheckEmailAsync(request.Email);
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
        public async Task<ActionResult<UserReadDto>> UploadAvatarAsync([FromForm] UserForm userForm)
        {
            var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userClaims == null)
            {
                throw new InvalidOperationException("Invalid user claims.");
            }
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
                    var updatedUser = await _userService.UploadAvatarAsync(userId, content);
                    if (updatedUser is not null)
                        return CreatedAtAction(nameof(UploadAvatarAsync), updatedUser.Id, updatedUser);

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
    public class EmailRequest
    {
        public string Email { get; set; }
    }
}