using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tarea__2.Models
{
    public class ArticuloEntity
    {
        [Key]
        public int Id { get; set; }
        
		public int IdClaseArticulo { get; set; }

		public string Codigo { get; set; }

		public string Nombre { get; set; }

        public System.Decimal Precio { get; set; }

    }
}
