using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TechStore.Entidades;

namespace TechStore.DAL
{
    public class UsuarioDAL
    {
        private readonly Conexion _conexion = new Conexion();

        /// <summary>
        /// Obtiene un usuario activo por su nombre de usuario (para validación de credenciales en BLL).
        /// </summary>
        public Usuario ObtenerPorNombreUsuario(string nombreUsuario)
        {
            using (SqlConnection cn = _conexion.Conectar())
            {
                using (SqlCommand cmd = new SqlCommand("SP_ObtenerUsuarioPorLogin", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@NombreUsuario", SqlDbType.VarChar, 50).Value = nombreUsuario;

                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return new Usuario
                            {
                                IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                NombreUsuario = dr["NombreUsuario"].ToString(),
                                ClaveHash = dr["ClaveHash"].ToString(),
                                ClaveSalt = dr["ClaveSalt"].ToString(),
                                NombreCompleto = dr["NombreCompleto"].ToString(),
                                Rol = dr["Rol"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"]),
                                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"])
                            };
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Lista todos los usuarios registrados en el sistema.
        /// </summary>
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();

            using (SqlConnection cn = _conexion.Conectar())
            {
                string sql = "SELECT IdUsuario, NombreUsuario, ClaveHash, ClaveSalt, NombreCompleto, Rol, Estado, FechaCreacion FROM Usuarios ORDER BY NombreCompleto ASC";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Usuario
                            {
                                IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                NombreUsuario = dr["NombreUsuario"].ToString(),
                                ClaveHash = dr["ClaveHash"].ToString(),
                                ClaveSalt = dr["ClaveSalt"].ToString(),
                                NombreCompleto = dr["NombreCompleto"].ToString(),
                                Rol = dr["Rol"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"]),
                                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"])
                            });
                        }
                    }
                }
            }

            return lista;
        }
    }
}

