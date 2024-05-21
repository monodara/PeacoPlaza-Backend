using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using Server.Core.src.Common;
using Server.Service.src.DTO;
using Server.Service.src.ServiceAbstract.AuthServiceAbstract;

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
            return CreatedAtAction(nameof(LoginAsync), token);
        }
        catch (AuthenticationException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
