using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tarea__2.Models
{
    public class ClaseArticuloEntity
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}
