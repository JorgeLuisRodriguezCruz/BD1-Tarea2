using System.ComponentModel.DataAnnotations;

namespace Tarea__2.Models
{
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Contraseña { get; set; }

    }
}
