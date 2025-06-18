using LudiSST.Data;
using LudiSST.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class RolController : ControllerBase
{
    private readonly LudiSSTContext _context;

    public RolController(LudiSSTContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CrearRol([FromBody] string nombre)
    {
        var rol = new Rol { Nombre = nombre };
        _context.Roles.Add(rol);
        await _context.SaveChangesAsync();
        return Ok(rol);
    }
}
