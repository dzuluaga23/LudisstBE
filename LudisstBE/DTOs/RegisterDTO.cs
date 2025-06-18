namespace LudisstBE.DTOs
{
    public class RegisterDTO
    {
        public int Documento { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string? Telefono { get; set; }
        public string Password { get; set; }
        public int RolId { get; set; }
    }
}
