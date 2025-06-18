using LudisstBE.DTOs;

namespace LudisstBE.Services
{
    public interface IAuthService
    {
        Task<(int statusCode, object result)> RegisterAsync(RegisterDTO dto);
        Task<(int statusCode, object result)> LoginAsync(LoginDTO dto);
    }
}
