using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TechStore.Entidades;

namespace TechStore.DAL
{
    public class VentaDAL
    {
        private readonly Conexion _conexion = new Conexion();

        /// <summary>
        /// Registra una venta completa (Cabecera y Detalles) de forma estrictamente ATÓMICA
        /// mediante ADO.NET SqlTransaction.
        /// Si cualquier validación o inserción falla (o si el stock es insuficiente),
        /// se ejecuta Rollback total garantizando CERO datos huérfanos.
        /// </summary>
        public int RegistrarVenta(Venta venta, List<DetalleVenta> detalles)
        {
            if (venta == null)
                throw new ArgumentNullException(nameof(venta), "La información de la cabecera de venta no puede ser nula.");

            if (detalles == null || detalles.Count == 0)
                throw new InvalidOperationException("No se puede registrar una venta sin productos en el detalle.");

            using (SqlConnection cn = _conexion.Conectar())
            {
                cn.Open();

                // INICIO DE LA TRANSACCIÓN EXPLÍCITA (SqlTransaction)
                using (SqlTransaction trans = cn.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        // -------------------------------------------------------------
                        // PASO 1: VALIDACIÓN Y ACTUALIZACIÓN ATÓMICA DE STOCK
                        // -------------------------------------------------------------
                        foreach (var det in detalles)
                        {
                            if (det.Cantidad <= 0)
                                throw new InvalidOperationException("La cantidad a vender del producto debe ser mayor a cero.");

                            // Consultar stock actual bloqueando la fila para evitar concurrencia sucia
                            string sqlCheckStock = @"SELECT Stock, Nombre, Precio 
                                                     FROM Productos WITH (UPDLOCK, ROWLOCK) 
                                                     WHERE IdProducto = @IdProducto AND Estado = 1";

                            using (SqlCommand cmdStock = new SqlCommand(sqlCheckStock, cn, trans))
                            {
                                cmdStock.Parameters.Add("@IdProducto", SqlDbType.Int).Value = det.IdProducto;

                                using (SqlDataReader dr = cmdStock.ExecuteReader())
                                {
                                    if (!dr.Read())
                                    {
                                        throw new InvalidOperationException(string.Format("El producto con ID {0} no existe o se encuentra inactivo.", det.IdProducto));
                                    }

                                    int stockDisponible = Convert.ToInt32(dr["Stock"]);
                                    string nombreProducto = dr["Nombre"].ToString();

                                    // Si el stock llegara a ser negativo, la transacción DEBE abortarse
                                    if (stockDisponible < det.Cantidad)
                                    {
                                        throw new InvalidOperationException(
                                            string.Format("Stock insuficiente para el producto '{0}'. Stock disponible: {1}, Cantidad solicitada: {2}. Se abortó la operación.",
                                            nombreProducto, stockDisponible, det.Cantidad));
                                    }
                                }
                            }

                            // Descontar la cantidad vendida del inventario dentro de la misma transacción
                            string sqlUpdateStock = @"UPDATE Productos 
                                                      SET Stock = Stock - @Cantidad 
                                                      WHERE IdProducto = @IdProducto";

                            using (SqlCommand cmdUpdate = new SqlCommand(sqlUpdateStock, cn, trans))
                            {
                                cmdUpdate.Parameters.Add("@Cantidad", SqlDbType.Int).Value = det.Cantidad;
                                cmdUpdate.Parameters.Add("@IdProducto", SqlDbType.Int).Value = det.IdProducto;

                                int filasAfectadas = cmdUpdate.ExecuteNonQuery();
                                if (filasAfectadas == 0)
                                {
                                    throw new InvalidOperationException(string.Format("No se pudo actualizar el inventario del producto ID {0}.", det.IdProducto));
                                }
                            }
                        }

                        // -------------------------------------------------------------
                        // PASO 2: GENERACIÓN DE CORRELATIVO DE FACTURA
                        // -------------------------------------------------------------
                        string numeroFactura = venta.NumeroFactura;
                        if (string.IsNullOrWhiteSpace(numeroFactura))
                        {
                            string sqlCorrelativo = "SELECT ISNULL(MAX(IdVenta), 0) + 1 FROM Ventas WITH (UPDLOCK)";
                            using (SqlCommand cmdCorrelativo = new SqlCommand(sqlCorrelativo, cn, trans))
                            {
                                int siguienteId = Convert.ToInt32(cmdCorrelativo.ExecuteScalar());
                                numeroFactura = string.Format("FAC-{0:D6}", siguienteId);
                            }
                        }

                        // -------------------------------------------------------------
                        // PASO 3: INSERTAR CABECERA DE LA VENTA (Tabla Ventas)
                        // Campos según enunciado: IdVenta, Fecha, Total, IdUsuario, IdProducto
                        // -------------------------------------------------------------
                        int? primerProductoId = venta.IdProducto.HasValue ? venta.IdProducto : (detalles.Count > 0 ? (int?)detalles[0].IdProducto : null);

                        string sqlVenta = @"INSERT INTO Ventas (NumeroFactura, Fecha, Total, IdUsuario, IdProducto, Estado) 
                                            VALUES (@NumeroFactura, @Fecha, @Total, @IdUsuario, @IdProducto, @Estado);
                                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        int idVentaGenerada;
                        using (SqlCommand cmdVenta = new SqlCommand(sqlVenta, cn, trans))
                        {
                            cmdVenta.Parameters.Add("@NumeroFactura", SqlDbType.VarChar, 20).Value = numeroFactura;
                            cmdVenta.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = venta.Fecha != DateTime.MinValue ? venta.Fecha : DateTime.Now;
                            cmdVenta.Parameters.Add("@Total", SqlDbType.Decimal).Value = venta.Total;
                            cmdVenta.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = venta.IdUsuario;
                            cmdVenta.Parameters.Add("@IdProducto", SqlDbType.Int).Value = (object)primerProductoId ?? DBNull.Value;
                            cmdVenta.Parameters.Add("@Estado", SqlDbType.VarChar, 20).Value = string.IsNullOrWhiteSpace(venta.Estado) ? "COMPLETADA" : venta.Estado;

                            idVentaGenerada = Convert.ToInt32(cmdVenta.ExecuteScalar());
                        }

                        // -------------------------------------------------------------
                        // PASO 4: INSERTAR DETALLES DE LA VENTA (Tabla DetalleVentas)
                        // Campos según enunciado: IdDetalle, IdVenta, IdProducto, Cantidad, PrecioUnitario, Subtotal
                        // -------------------------------------------------------------
                        string sqlDetalle = @"INSERT INTO DetalleVentas (IdVenta, IdProducto, Cantidad, PrecioUnitario, Subtotal)
                                              VALUES (@IdVenta, @IdProducto, @Cantidad, @PrecioUnitario, @Subtotal);";

                        foreach (var det in detalles)
                        {
                            using (SqlCommand cmdDet = new SqlCommand(sqlDetalle, cn, trans))
                            {
                                cmdDet.Parameters.Add("@IdVenta", SqlDbType.Int).Value = idVentaGenerada;
                                cmdDet.Parameters.Add("@IdProducto", SqlDbType.Int).Value = det.IdProducto;
                                cmdDet.Parameters.Add("@Cantidad", SqlDbType.Int).Value = det.Cantidad;
                                cmdDet.Parameters.Add("@PrecioUnitario", SqlDbType.Decimal).Value = det.PrecioUnitario;
                                cmdDet.Parameters.Add("@Subtotal", SqlDbType.Decimal).Value = det.Subtotal;

                                cmdDet.ExecuteNonQuery();
                            }
                        }

                        // -------------------------------------------------------------
                        // PASO 5: CONFIRMACIÓN EXITOSA DE TODAS LAS OPERACIONES (COMMIT)
                        // -------------------------------------------------------------
                        trans.Commit();
                        return idVentaGenerada;
                    }
                    catch (Exception)
                    {
                        // ---------------------------------------------------------
                        // ROLLBACK TOTAL ANTE CUALQUIER FALLA: 0 DATOS HUÉRFANOS
                        // ---------------------------------------------------------
                        try
                        {
                            trans.Rollback();
                        }
                        catch
                        {
                            // Si la conexión se cerró inesperadamente, el Rollback ocurre automáticamente en el servidor
                        }

                        // Propaga la excepción para ser capturada con try-catch en capas superiores
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Lista el historial de ventas registradas.
        /// </summary>
        public List<Venta> ListarVentas()
        {
            List<Venta> lista = new List<Venta>();

            using (SqlConnection cn = _conexion.Conectar())
            {
                using (SqlCommand cmd = new SqlCommand("SP_ListarVentas", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Venta
                            {
                                IdVenta = Convert.ToInt32(dr["IdVenta"]),
                                NumeroFactura = dr["NumeroFactura"].ToString(),
                                Fecha = Convert.ToDateTime(dr["Fecha"]),
                                Total = Convert.ToDecimal(dr["Total"]),
                                IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                Cajero = dr["Cajero"].ToString(),
                                IdProducto = dr["IdProducto"] != DBNull.Value ? (int?)Convert.ToInt32(dr["IdProducto"]) : null,
                                Estado = dr["Estado"].ToString(),
                                CantidadItems = Convert.ToInt32(dr["CantidadItems"])
                            });
                        }
                    }
                }
            }

            return lista;
        }

        /// <summary>
        /// Obtiene los detalles de una venta específica.
        /// </summary>
        public List<DetalleVenta> ObtenerDetallesPorVenta(int idVenta)
        {
            List<DetalleVenta> lista = new List<DetalleVenta>();

            using (SqlConnection cn = _conexion.Conectar())
            {
                using (SqlCommand cmd = new SqlCommand("SP_ObtenerDetallesPorVenta", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdVenta", SqlDbType.Int).Value = idVenta;

                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new DetalleVenta
                            {
                                IdDetalle = Convert.ToInt32(dr["IdDetalle"]),
                                IdVenta = Convert.ToInt32(dr["IdVenta"]),
                                IdProducto = Convert.ToInt32(dr["IdProducto"]),
                                CodigoProducto = dr["CodigoProducto"].ToString(),
                                NombreProducto = dr["NombreProducto"].ToString(),
                                Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                PrecioUnitario = Convert.ToDecimal(dr["PrecioUnitario"]),
                                Subtotal = Convert.ToDecimal(dr["Subtotal"])
                            });
                        }
                    }
                }
            }

            return lista;
        }
    }
}

