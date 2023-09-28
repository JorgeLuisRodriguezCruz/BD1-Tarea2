namespace Tarea__2.Models
{
    public class ArticuloVista
    {
        public string ClaseArticulo { get; set; }

        public string Codigo { get; set; }

        public string Nombre { get; set; }

        public System.Decimal Precio { get; set; }

        public ArticuloVista (string claseArticulo, string codigo, string nombre, decimal precio)
        {
            ClaseArticulo = claseArticulo;
            Codigo = codigo;
            Nombre = nombre;
            Precio = precio;
        }
    }
}
