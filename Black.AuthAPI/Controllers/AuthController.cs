using Black.Model.Authentication;
using Black.Service.AuthService;
using DotNetCore.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Black.AuthAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    [AllowAnonymous]
    [HttpPost]
    public IActionResult SignIn(SignInModel model) => _authService.SignInAsync(model).ApiResult();
}
