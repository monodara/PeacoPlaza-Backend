using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using Server.Core.src.Common;
using Server.Service.src.DTO;
using Server.Service.src.ServiceAbstract.AuthServiceAbstract;
using Server.Service.src.Shared;

namespace Server.Controller.src.Controller;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("api/v1/auth/login")]
    public async Task<ActionResult<string>> LoginAsync([FromBody] UserCredential userCredential)
    {
        try
        {
            var token = await _authService.LoginAsync(userCredential);
            return Ok(token); // Return 200 OK with the token
        }
        catch (CustomException ex)
        {
            return StatusCode(ex.StatusCode, ex.Message); // Use the StatusCode from CustomException
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message); // Return 500 Internal Server Error for general exceptions
        }
    }
}
