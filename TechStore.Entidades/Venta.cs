using System;
using System.Collections.Generic;

namespace TechStore.Entidades
{
    /// <summary>
    /// Entidad que representa la Cabecera de la Venta (Tabla Ventas)
    /// </summary>
    public class Venta
    {
        public int IdVenta { get; set; }
        public string NumeroFactura { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public int IdUsuario { get; set; }
        public int? IdProducto { get; set; } // Requerido explícitamente en el enunciado: Cabecera (IdVenta, Fecha, Total, IdUsuario, IdProducto)
        public string Estado { get; set; }

        // Propiedades de navegación / lectura en interfaz
        public string NombreUsuario { get; set; }
        public string Cajero { get; set; }
        public int CantidadItems { get; set; }

        public List<DetalleVenta> Detalles { get; set; }

        public Venta()
        {
            Fecha = DateTime.Now;
            Estado = "COMPLETADA";
            Detalles = new List<DetalleVenta>();
        }
    }
}

