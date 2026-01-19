using Desafio.Api.Dtos.Request;
using Desafio.Api.Dtos.Response;
using Desafio.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Desafio.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login(AuthRequestDTO request)
    {
        var token = _authService.Authenticate(request);
        return Ok(new AuthResponseDTO(token));
    }
}
