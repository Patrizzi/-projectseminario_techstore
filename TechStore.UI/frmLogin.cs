using System;
using System.Drawing;
using System.Windows.Forms;
using TechStore.BLL;
using TechStore.Entidades;

namespace TechStore.UI
{
    public partial class frmLogin : Form
    {
        private readonly UsuarioBLL _usuarioBLL = new UsuarioBLL();

        public frmLogin()
        {
            InitializeComponent();
            this.BackColor = Tema.Canvas;
            Tema.Aplicar(this);
            btnIngresar.BackColor = Tema.TextPrimary;
            lblHeaderTitle.Text = "TechStore POS";
            lblNodeBadge.Visible = true;
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string clave = txtClave.Text;

            try
            {
                // Validación con algoritmo SHA-256 + Salt en la Capa de Negocio (BLL)
                Usuario usuarioAutenticado = _usuarioBLL.ValidarLogin(usuario, clave);

                // Ocultar formulario de Login mientras está abierta la sesión del POS
                this.Hide();

                using (frmPrincipal frm = new frmPrincipal())
                {
                    frm.ShowDialog();
                }

                // Al retornar de la ventana principal
                txtClave.Clear();
                txtUsuario.Focus();
                this.Show();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Control de Acceso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtClave.Focus();
                txtClave.SelectAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo iniciar sesión. Verifique su conexión al sistema.\nDetalle: " + ex.Message,
                                "Error del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void chkMostrarClave_CheckedChanged(object sender, EventArgs e)
        {
            txtClave.UseSystemPasswordChar = !chkMostrarClave.Checked;
        }

        private void btnDemoAdmin_Click(object sender, EventArgs e)
        {
            txtUsuario.Text = "admin";
            txtClave.Text = "admin123";
            btnIngresar.Focus();
        }

        private void btnDemoCajero_Click(object sender, EventArgs e)
        {
            txtUsuario.Text = "cajero1";
            txtClave.Text = "123456";
            btnIngresar.Focus();
        }

        private void btnDemoCajero2_Click(object sender, EventArgs e)
        {
            txtUsuario.Text = "cajero2";
            txtClave.Text = "123456";
            btnIngresar.Focus();
        }

        private void pnlCenterCard_Paint(object sender, PaintEventArgs e)
        {
            using (Pen p = new Pen(Color.FromArgb(226, 232, 240), 1))
            {
                e.Graphics.DrawRectangle(p, 0, 0, pnlCenterCard.Width - 1, pnlCenterCard.Height - 1);
            }
        }
    }
}
