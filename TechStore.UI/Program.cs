using System;
using System.Windows.Forms;

namespace TechStore.UI
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación TechStore POS Desktop.
        /// Inicia con la pantalla de autenticación y control de acceso (frmLogin).
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmLogin());
        }
    }
}

