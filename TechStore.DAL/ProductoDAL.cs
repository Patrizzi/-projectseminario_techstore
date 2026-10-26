using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TechStore.Entidades;

namespace TechStore.DAL
{
    public class ProductoDAL
    {
        private readonly Conexion _conexion = new Conexion();

        /// <summary>
        /// Lista todos los productos disponibles en inventario.
        /// </summary>
        public List<Producto> Listar(bool soloActivos = true)
        {
            List<Producto> lista = new List<Producto>();

            using (SqlConnection cn = _conexion.Conectar())
            {
                using (SqlCommand cmd = new SqlCommand("SP_ListarProductos", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@SoloActivos", SqlDbType.Bit).Value = soloActivos;

                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Producto
                            {
                                IdProducto = Convert.ToInt32(dr["IdProducto"]),
                                Codigo = dr["Codigo"].ToString(),
                                Nombre = dr["Nombre"].ToString(),
                                Descripcion = dr["Descripcion"] != DBNull.Value ? dr["Descripcion"].ToString() : string.Empty,
                                Precio = Convert.ToDecimal(dr["Precio"]),
                                Stock = Convert.ToInt32(dr["Stock"]),
                                IdCategoria = dr["IdCategoria"] != DBNull.Value ? (int?)Convert.ToInt32(dr["IdCategoria"]) : null,
                                NombreCategoria = dr["NombreCategoria"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"])
                            });
                        }
                    }
                }
            }

            return lista;
        }

        /// <summary>
        /// Obtiene un producto por su clave primaria.
        /// </summary>
        public Producto ObtenerPorId(int idProducto)
        {
            using (SqlConnection cn = _conexion.Conectar())
            {
                using (SqlCommand cmd = new SqlCommand("SP_ObtenerProductoPorId", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdProducto", SqlDbType.Int).Value = idProducto;

                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return new Producto
                            {
                                IdProducto = Convert.ToInt32(dr["IdProducto"]),
                                Codigo = dr["Codigo"].ToString(),
                                Nombre = dr["Nombre"].ToString(),
                                Descripcion = dr["Descripcion"] != DBNull.Value ? dr["Descripcion"].ToString() : string.Empty,
                                Precio = Convert.ToDecimal(dr["Precio"]),
                                Stock = Convert.ToInt32(dr["Stock"]),
                                IdCategoria = dr["IdCategoria"] != DBNull.Value ? (int?)Convert.ToInt32(dr["IdCategoria"]) : null,
                                NombreCategoria = dr["NombreCategoria"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"])
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}

