using System;
using System.Security.Cryptography;
using System.Text;

namespace TechStore.BLL
{
    /// <summary>
    /// Capa de Lógica de Negocio: Implementación del algoritmo de Hashing con SHA-256 + Salt
    /// para proteger las contraseñas de los usuarios en la base de datos (Requerimiento 4).
    /// </summary>
    public static class SeguridadBLL
    {
        /// <summary>
        /// Genera una sal criptográfica (Salt) aleatoria y segura.
        /// </summary>
        public static string GenerarSalt(int tamanoBytes = 16)
        {
            byte[] saltBytes = new byte[tamanoBytes];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        /// <summary>
        /// Genera el resumen hash SHA-256 de la combinación de la contraseña en texto plano y la sal.
        /// </summary>
        public static string GenerarHash(string contrasena, string salt)
        {
            if (string.IsNullOrEmpty(contrasena))
                throw new ArgumentException("La contraseña no puede estar vacía.", nameof(contrasena));

            if (string.IsNullOrEmpty(salt))
                throw new ArgumentException("La sal criptográfica es obligatoria.", nameof(salt));

            using (SHA256 sha256 = SHA256.Create())
            {
                // Concatenar contraseña con la sal
                byte[] bytesCombinados = Encoding.UTF8.GetBytes(contrasena + salt);
                byte[] hashBytes = sha256.ComputeHash(bytesCombinados);

                // Convertir a cadena hexadecimal sin guiones
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Valida si una contraseña ingresada coincide con el hash almacenado utilizando la sal correspondiente.
        /// </summary>
        public static bool VerificarHash(string contrasenaIngresada, string salt, string hashAlmacenado)
        {
            if (string.IsNullOrEmpty(contrasenaIngresada) || string.IsNullOrEmpty(salt) || string.IsNullOrEmpty(hashAlmacenado))
                return false;

            string hashGenerado = GenerarHash(contrasenaIngresada, salt);
            return string.Equals(hashGenerado, hashAlmacenado, StringComparison.OrdinalIgnoreCase);
        }
    }
}

