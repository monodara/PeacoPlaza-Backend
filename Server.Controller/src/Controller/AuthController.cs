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
    public async Task<string> LoginAsync([FromBody] UserCredential userCredential)
    {
        // return await _authService.LoginAsync(userCredential);
        return await _authService.Login(userCredential);
    }
}
