using System;

namespace TechStore.Entidades
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string ClaveHash { get; set; }
        public string ClaveSalt { get; set; }
        public string NombreCompleto { get; set; }
        public string Rol { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }

        public Usuario()
        {
            Estado = true;
            FechaCreacion = DateTime.Now;
        }
    }
}

