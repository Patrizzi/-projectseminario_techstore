using System;
using System.Collections.Generic;
using TechStore.DAL;
using TechStore.Entidades;

namespace TechStore.BLL
{
    /// <summary>
    /// Capa de Lógica de Negocio: Control de acceso, autenticación con Hashing y gestión del Singleton de sesión.
    /// </summary>
    public class UsuarioBLL
    {
        private readonly UsuarioDAL _usuarioDAL = new UsuarioDAL();

        /// <summary>
        /// Valida las credenciales del usuario aplicando el algoritmo de verificación de Hashing (SHA-256 con Salt).
        /// Si las credenciales son válidas, inicializa la instancia global del Singleton SesionUsuario.
        /// </summary>
        public Usuario ValidarLogin(string nombreUsuario, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                throw new ApplicationException("Por favor, ingrese el nombre de usuario.");

            if (string.IsNullOrWhiteSpace(contrasena))
                throw new ApplicationException("Por favor, ingrese la contraseña.");

            nombreUsuario = nombreUsuario.Trim();

            // 1. Obtener usuario de la capa de datos
            Usuario usuario = _usuarioDAL.ObtenerPorNombreUsuario(nombreUsuario);

            if (usuario == null)
            {
                // Mensaje genérico amigable para seguridad
                throw new ApplicationException("Acceso Denegado: El usuario o la contraseña ingresados son incorrectos.");
            }

            if (!usuario.Estado)
            {
                throw new ApplicationException("Acceso Denegado: La cuenta del usuario se encuentra inactiva. Contacte al administrador.");
            }

            // 2. Validar contraseña aplicando Hashing SHA-256 + Salt en la capa de negocio
            bool esContrasenaValida = SeguridadBLL.VerificarHash(contrasena, usuario.ClaveSalt, usuario.ClaveHash);

            if (!esContrasenaValida)
            {
                throw new ApplicationException("Acceso Denegado: El usuario o la contraseña ingresados son incorrectos.");
            }

            // 3. Inicializar el Singleton SesionUsuario (Thread-Safe) con los datos del usuario autenticado
            SesionUsuario.Instancia.IniciarSesion(
                usuario.IdUsuario,
                usuario.NombreUsuario,
                usuario.NombreCompleto,
                usuario.Rol
            );

            return usuario;
        }

        /// <summary>
        /// Cierra la sesión activa en el Singleton global.
        /// </summary>
        public void CerrarSesion()
        {
            SesionUsuario.Instancia.CerrarSesion();
        }

        /// <summary>
        /// Lista todos los usuarios registrados.
        /// </summary>
        public List<Usuario> ListarUsuarios()
        {
            return _usuarioDAL.Listar();
        }
    }
}

