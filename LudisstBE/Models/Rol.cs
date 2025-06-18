using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LudiSST.Models
{
    public class Rol
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Nombre { get; set; }
        public ICollection<Usuario>? Usuarios { get; set; }
    }
}
