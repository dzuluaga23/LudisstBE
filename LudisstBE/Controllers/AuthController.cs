using LudisstBE.DTOs;
using LudisstBE.Services;
using Microsoft.AspNetCore.Mvc;

namespace LudisstBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("RegistrarUsuario")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            var (status, result) = await _authService.RegisterAsync(dto);
            return StatusCode(status, result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var (status, result) = await _authService.LoginAsync(dto);
            return StatusCode(status, result);
        }
    }
}
