using System;
using System.Configuration;
using System.Data.SqlClient;

namespace TechStore.DAL
{
    public class Conexion
    {
        private const string ConnectionStringName = "cn_techstore";
        private const string CadenaPorDefecto = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=bd_techstore; Integrated Security=true;";

        public SqlConnection Conectar()
        {
            string connectionString = CadenaPorDefecto;

            try
            {
                var setting = ConfigurationManager.ConnectionStrings[ConnectionStringName];
                if (setting != null && !string.IsNullOrWhiteSpace(setting.ConnectionString))
                {
                    connectionString = setting.ConnectionString;
                }
            }
            catch
            {
                // En caso de que no se cargue App.config, usa la cadena por defecto a LocalDB
                connectionString = CadenaPorDefecto;
            }

            return new SqlConnection(connectionString);
        }
    }
}

