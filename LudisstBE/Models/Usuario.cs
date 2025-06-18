using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LudiSST.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Documento { get; set; }
        [Required, MaxLength(60)]
        public string Nombre { get; set; }
        [Required, MaxLength(60)]
        public string Apellido { get; set; }
        [Required, MaxLength(100)]
        public string Email { get; set; }
        [MaxLength(20)]
        public string? Telefono { get; set; }
        [Required, MaxLength(255)]
        public string PasswordHash { get; set; }
        [MaxLength(255)]
        public string? PasswordSalt { get; set; }
        [Required]
        public int RolId { get; set; }
        [ForeignKey("RolId")]
        public Rol Rol { get; set; }
        public bool Estado { get; set; }
    }
}
