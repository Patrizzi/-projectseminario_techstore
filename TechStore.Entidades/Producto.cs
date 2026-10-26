namespace TechStore.Entidades
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int? IdCategoria { get; set; }
        public string NombreCategoria { get; set; }
        public bool Estado { get; set; }

        public Producto()
        {
            Estado = true;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} (Stock: {2} | S/. {3:N2})", Codigo, Nombre, Stock, Precio);
        }
    }
}

