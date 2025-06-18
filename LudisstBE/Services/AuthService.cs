using LudiSST.Data;
using LudiSST.Models;
using LudisstBE.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LudisstBE.Services
{
    public class AuthService : IAuthService
    {
        private readonly LudiSSTContext db;
        private readonly IConfiguration _config;

        public AuthService(LudiSSTContext context, IConfiguration config)
        {
            db = context;
            _config = config;
        }

        public async Task<(int statusCode, object result)> RegisterAsync(RegisterDTO dto)
        {
            try
            {
                if (await db.Usuarios.AnyAsync(u => u.Email == dto.Email))
                    return (400, new { error = "El correo ya está registrado" });

                var rol = await db.Roles.FindAsync(dto.RolId);
                if (rol == null)
                    return (400, new { error = "Rol inválido" });

                var salt = BCrypt.Net.BCrypt.GenerateSalt();
                var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password, salt);

                var user = new Usuario
                {
                    Documento = dto.Documento,
                    Nombre = dto.Nombre,
                    Apellido = dto.Apellido,
                    Email = dto.Email,
                    Telefono = dto.Telefono,
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    RolId = dto.RolId,
                    Estado = true
                };

                db.Usuarios.Add(user);
                await db.SaveChangesAsync();

                return (200, new { message = "Usuario registrado exitosamente" });
            }
            catch (Exception ex)
            {
                return (500, new { error = "Error interno del servidor", detalle = ex.Message });
            }
        }

        public async Task<(int statusCode, object result)> LoginAsync(LoginDTO dto)
        {
            try
            {
                var user = await db.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Email == dto.Email);

                if (user == null || !user.Estado)
                    return (401, new { error = "Credenciales inválidas o usuario inactivo" });

                var isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
                if (!isValid)
                    return (401, new { error = "Contraseña incorrecta" });

                var token = GenerateJwtToken(user);

                return (200, new
                {
                    token,
                    usuario = new
                    {
                        user.Id,
                        user.Nombre,
                        user.Apellido,
                        user.Email,
                    }
                });
            }
            catch (Exception ex)
            {
                return (500, new { error = "Error interno del servidor", detalle = ex.Message });
            }
        }

        private string GenerateJwtToken(Usuario user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("id", user.Id.ToString()),
                new Claim("rol", user.Rol.Nombre)
            };

            var keyString = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(keyString) || Encoding.UTF8.GetBytes(keyString).Length < 32)
                throw new Exception("La clave JWT es muy corta. Debe tener al menos 256 bits (32 bytes)");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(1);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
