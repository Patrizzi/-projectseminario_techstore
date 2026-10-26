using System;
using System.Collections.Generic;
using TechStore.DAL;
using TechStore.Entidades;

namespace TechStore.BLL
{
    /// <summary>
    /// Capa de Lógica de Negocio: Validaciones previas a la transacción y coordinación del registro de ventas.
    /// </summary>
    public class VentaBLL
    {
        private readonly VentaDAL _ventaDAL = new VentaDAL();

        /// <summary>
        /// Procesa y registra una venta en el sistema realizando todas las validaciones de negocio necesarias.
        /// </summary>
        public int RegistrarVenta(Venta venta, List<DetalleVenta> detalles)
        {
            // 1. Validación de Cabecera
            if (venta == null)
                throw new ApplicationException("Los datos de la venta no han sido proporcionados.");

            // 2. Validación crítica de Rúbrica: No se deben guardar ventas sin productos
            if (detalles == null || detalles.Count == 0)
                throw new ApplicationException("No es posible registrar una venta sin productos. Agregue al menos un producto al carrito.");

            // 3. Validación de Usuario Activo en Sesión
            if (venta.IdUsuario <= 0)
            {
                if (SesionUsuario.Instancia.HaySesionActiva)
                {
                    venta.IdUsuario = SesionUsuario.Instancia.IdUsuario;
                }
                else
                {
                    throw new ApplicationException("No hay una sesión activa de cajero. Inicie sesión para continuar.");
                }
            }

            // 4. Validación de Líneas de Detalle y Cálculo de Subtotales
            decimal sumaSubtotales = 0m;
            foreach (var item in detalles)
            {
                if (item.IdProducto <= 0)
                    throw new ApplicationException("Cada elemento del detalle debe contar con un producto válido.");

                // Validación de Rúbrica: No permitir vender 0 cantidades ni números negativos
                if (item.Cantidad <= 0)
                    throw new ApplicationException(string.Format("La cantidad para el producto '{0}' debe ser mayor a cero.", item.NombreProducto ?? ("ID " + item.IdProducto)));

                if (item.PrecioUnitario < 0)
                    throw new ApplicationException(string.Format("El precio unitario para el producto '{0}' no puede ser negativo.", item.NombreProducto ?? ("ID " + item.IdProducto)));

                item.CalcularSubtotal();
                sumaSubtotales += item.Subtotal;
            }

            // Asignar el total recalculado para garantizar consistencia matemática
            venta.Total = sumaSubtotales;

            // 5. Delegar a la Capa de Datos para la ejecución de la Transacción Atómica (SqlTransaction)
            try
            {
                return _ventaDAL.RegistrarVenta(venta, detalles);
            }
            catch (Exception ex)
            {
                // Encapsular en mensaje amigable para que la UI no reciba errores crudos de SQL
                throw new ApplicationException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Consulta el historial completo de ventas realizadas.
        /// </summary>
        public List<Venta> ListarVentas()
        {
            return _ventaDAL.ListarVentas();
        }

        /// <summary>
        /// Obtiene los detalles de los productos incluidos en una venta dada.
        /// </summary>
        public List<DetalleVenta> ObtenerDetallesPorVenta(int idVenta)
        {
            if (idVenta <= 0)
                throw new ArgumentException("El ID de la venta debe ser mayor a cero.", nameof(idVenta));

            return _ventaDAL.ObtenerDetallesPorVenta(idVenta);
        }
    }
}

