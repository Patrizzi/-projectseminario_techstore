using System;

namespace TechStore.Entidades
{
    /// <summary>
    /// Clase que implementa el patrón de diseño Singleton (Thread-Safe con Double-Checked Locking)
    /// para almacenar de forma centralizada la sesión del usuario autenticado en la aplicación.
    /// </summary>
    public sealed class SesionUsuario
    {
        // 1. Instancia única estática privada
        private static SesionUsuario _instancia = null;

        // 2. Objeto de bloqueo para garantizar la concurrencia segura (Thread-Safe)
        private static readonly object _bloqueo = new object();

        // Propiedades de la sesión activa
        public int IdUsuario { get; private set; }
        public string NombreUsuario { get; private set; }
        public string NombreCompleto { get; private set; }
        public string Rol { get; private set; }
        public DateTime? FechaInicioSesion { get; private set; }

        /// <summary>
        /// Constructor privado estricto: Impide que cualquier otra clase use 'new SesionUsuario()'.
        /// </summary>
        private SesionUsuario()
        {
            Limpiar();
        }

        /// <summary>
        /// Acceso global a la instancia única mediante Double-Checked Locking para máxima eficiencia y seguridad de hilos.
        /// </summary>
        public static SesionUsuario Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (_bloqueo)
                    {
                        if (_instancia == null)
                        {
                            _instancia = new SesionUsuario();
                        }
                    }
                }
                return _instancia;
            }
        }

        /// <summary>
        /// Registra la información del usuario autenticado en la sesión global.
        /// </summary>
        public void IniciarSesion(int idUsuario, string nombreUsuario, string nombreCompleto, string rol)
        {
            lock (_bloqueo)
            {
                this.IdUsuario = idUsuario;
                this.NombreUsuario = nombreUsuario;
                this.NombreCompleto = nombreCompleto;
                this.Rol = rol;
                this.FechaInicioSesion = DateTime.Now;
            }
        }

        /// <summary>
        /// Cierra la sesión activa y reinicia los valores.
        /// </summary>
        public void CerrarSesion()
        {
            lock (_bloqueo)
            {
                Limpiar();
            }
        }

        private void Limpiar()
        {
            this.IdUsuario = 0;
            this.NombreUsuario = string.Empty;
            this.NombreCompleto = string.Empty;
            this.Rol = string.Empty;
            this.FechaInicioSesion = null;
        }

        /// <summary>
        /// Indica si existe una sesión actualmente activa.
        /// </summary>
        public bool HaySesionActiva
        {
            get
            {
                return IdUsuario > 0;
            }
        }
    }
}

