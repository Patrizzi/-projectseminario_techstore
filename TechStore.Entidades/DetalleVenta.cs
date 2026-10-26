namespace TechStore.Entidades
{
    /// <summary>
    /// Entidad que representa cada línea de detalle de la Venta (Tabla DetalleVentas)
    /// </summary>
    public class DetalleVenta
    {
        public int IdDetalle { get; set; }
        public int IdVenta { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }

        // Propiedades descriptivas para visualización en DataGridView de la UI
        public string CodigoProducto { get; set; }
        public string NombreProducto { get; set; }

        public DetalleVenta()
        {
        }

        public DetalleVenta(int idProducto, string codigo, string nombre, int cantidad, decimal precioUnitario)
        {
            this.IdProducto = idProducto;
            this.CodigoProducto = codigo;
            this.NombreProducto = nombre;
            this.Cantidad = cantidad;
            this.PrecioUnitario = precioUnitario;
            this.Subtotal = cantidad * precioUnitario;
        }

        public void CalcularSubtotal()
        {
            Subtotal = Cantidad * PrecioUnitario;
        }
    }
}

