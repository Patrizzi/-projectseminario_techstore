namespace TechStore.UI
{
    partial class frmLogin
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlCenterCard = new System.Windows.Forms.Panel();
            this.lblInfoSeguridad = new System.Windows.Forms.Label();
            this.grpDemo = new System.Windows.Forms.GroupBox();
            this.btnDemoCajero2 = new System.Windows.Forms.Button();
            this.btnDemoCajero = new System.Windows.Forms.Button();
            this.btnDemoAdmin = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.btnIngresar = new System.Windows.Forms.Button();
            this.chkMostrarClave = new System.Windows.Forms.CheckBox();
            this.txtClave = new System.Windows.Forms.TextBox();
            this.lblClave = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.pnlCardHeader = new System.Windows.Forms.Panel();
            this.pnlLoginLogo = new System.Windows.Forms.Panel();
            this.lblLoginLogo = new System.Windows.Forms.Label();
            this.lblHeaderSubtitle = new System.Windows.Forms.Label();
            this.lblNodeBadge = new System.Windows.Forms.Label();
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.pnlCenterCard.SuspendLayout();
            this.grpDemo.SuspendLayout();
            this.pnlCardHeader.SuspendLayout();
            this.pnlLoginLogo.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCenterCard
            // 
            this.pnlCenterCard.BackColor = System.Drawing.Color.White;
            this.pnlCenterCard.Controls.Add(this.lblInfoSeguridad);
            this.pnlCenterCard.Controls.Add(this.grpDemo);
            this.pnlCenterCard.Controls.Add(this.btnSalir);
            this.pnlCenterCard.Controls.Add(this.btnIngresar);
            this.pnlCenterCard.Controls.Add(this.chkMostrarClave);
            this.pnlCenterCard.Controls.Add(this.txtClave);
            this.pnlCenterCard.Controls.Add(this.lblClave);
            this.pnlCenterCard.Controls.Add(this.txtUsuario);
            this.pnlCenterCard.Controls.Add(this.lblUsuario);
            this.pnlCenterCard.Controls.Add(this.pnlCardHeader);
            this.pnlCenterCard.Location = new System.Drawing.Point(35, 30);
            this.pnlCenterCard.Name = "pnlCenterCard";
            this.pnlCenterCard.Padding = new System.Windows.Forms.Padding(24);
            this.pnlCenterCard.Size = new System.Drawing.Size(430, 485);
            this.pnlCenterCard.TabIndex = 0;
            this.pnlCenterCard.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCenterCard_Paint);
            // 
            // lblInfoSeguridad
            // 
            this.lblInfoSeguridad.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblInfoSeguridad.Font = new System.Drawing.Font("Consolas", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfoSeguridad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblInfoSeguridad.Location = new System.Drawing.Point(24, 441);
            this.lblInfoSeguridad.Name = "lblInfoSeguridad";
            this.lblInfoSeguridad.Size = new System.Drawing.Size(382, 20);
            this.lblInfoSeguridad.TabIndex = 9;
            this.lblInfoSeguridad.Text = "HASHING SHA-256 CON SALT (BLL) • SINGLETON THREAD-SAFE";
            this.lblInfoSeguridad.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpDemo
            // 
            this.grpDemo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.grpDemo.Controls.Add(this.btnDemoCajero2);
            this.grpDemo.Controls.Add(this.btnDemoCajero);
            this.grpDemo.Controls.Add(this.btnDemoAdmin);
            this.grpDemo.Font = new System.Drawing.Font("Consolas", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpDemo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.grpDemo.Location = new System.Drawing.Point(24, 328);
            this.grpDemo.Name = "grpDemo";
            this.grpDemo.Size = new System.Drawing.Size(382, 95);
            this.grpDemo.TabIndex = 8;
            this.grpDemo.TabStop = false;
            this.grpDemo.Text = " PREAJUSTES DE EVALUACIÓN (AUTOCOMPLETAR) ";
            // 
            // btnDemoCajero2
            // 
            this.btnDemoCajero2.BackColor = System.Drawing.Color.White;
            this.btnDemoCajero2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDemoCajero2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnDemoCajero2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDemoCajero2.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDemoCajero2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.btnDemoCajero2.Location = new System.Drawing.Point(256, 26);
            this.btnDemoCajero2.Name = "btnDemoCajero2";
            this.btnDemoCajero2.Size = new System.Drawing.Size(115, 52);
            this.btnDemoCajero2.TabIndex = 2;
            this.btnDemoCajero2.Text = "👤 Cajero 2\r\ncajero2 / 123456";
            this.btnDemoCajero2.UseVisualStyleBackColor = false;
            this.btnDemoCajero2.Click += new System.EventHandler(this.btnDemoCajero2_Click);
            // 
            // btnDemoCajero
            // 
            this.btnDemoCajero.BackColor = System.Drawing.Color.White;
            this.btnDemoCajero.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDemoCajero.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnDemoCajero.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDemoCajero.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDemoCajero.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.btnDemoCajero.Location = new System.Drawing.Point(133, 26);
            this.btnDemoCajero.Name = "btnDemoCajero";
            this.btnDemoCajero.Size = new System.Drawing.Size(115, 52);
            this.btnDemoCajero.TabIndex = 1;
            this.btnDemoCajero.Text = "👤 Cajero 1\r\ncajero1 / 123456";
            this.btnDemoCajero.UseVisualStyleBackColor = false;
            this.btnDemoCajero.Click += new System.EventHandler(this.btnDemoCajero_Click);
            // 
            // btnDemoAdmin
            // 
            this.btnDemoAdmin.BackColor = System.Drawing.Color.White;
            this.btnDemoAdmin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDemoAdmin.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnDemoAdmin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDemoAdmin.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDemoAdmin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.btnDemoAdmin.Location = new System.Drawing.Point(10, 26);
            this.btnDemoAdmin.Name = "btnDemoAdmin";
            this.btnDemoAdmin.Size = new System.Drawing.Size(115, 52);
            this.btnDemoAdmin.TabIndex = 0;
            this.btnDemoAdmin.Text = "👑 Admin\r\nadmin / admin123";
            this.btnDemoAdmin.UseVisualStyleBackColor = false;
            this.btnDemoAdmin.Click += new System.EventHandler(this.btnDemoAdmin_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.BackColor = System.Drawing.Color.White;
            this.btnSalir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSalir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalir.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.btnSalir.Location = new System.Drawing.Point(276, 275);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(130, 38);
            this.btnSalir.TabIndex = 7;
            this.btnSalir.Text = "Cerrar";
            this.btnSalir.UseVisualStyleBackColor = false;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // btnIngresar
            // 
            this.btnIngresar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.btnIngresar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnIngresar.FlatAppearance.BorderSize = 0;
            this.btnIngresar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIngresar.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIngresar.ForeColor = System.Drawing.Color.White;
            this.btnIngresar.Location = new System.Drawing.Point(24, 275);
            this.btnIngresar.Name = "btnIngresar";
            this.btnIngresar.Size = new System.Drawing.Size(242, 38);
            this.btnIngresar.TabIndex = 6;
            this.btnIngresar.Text = "Iniciar Sesión";
            this.btnIngresar.UseVisualStyleBackColor = false;
            this.btnIngresar.Click += new System.EventHandler(this.btnIngresar_Click);
            // 
            // chkMostrarClave
            // 
            this.chkMostrarClave.AutoSize = true;
            this.chkMostrarClave.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkMostrarClave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.chkMostrarClave.Location = new System.Drawing.Point(24, 240);
            this.chkMostrarClave.Name = "chkMostrarClave";
            this.chkMostrarClave.Size = new System.Drawing.Size(127, 19);
            this.chkMostrarClave.TabIndex = 5;
            this.chkMostrarClave.Text = "Mostrar contraseña";
            this.chkMostrarClave.UseVisualStyleBackColor = true;
            this.chkMostrarClave.CheckedChanged += new System.EventHandler(this.chkMostrarClave_CheckedChanged);
            // 
            // txtClave
            // 
            this.txtClave.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtClave.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtClave.Location = new System.Drawing.Point(24, 204);
            this.txtClave.MaxLength = 50;
            this.txtClave.Name = "txtClave";
            this.txtClave.Size = new System.Drawing.Size(382, 26);
            this.txtClave.TabIndex = 4;
            this.txtClave.UseSystemPasswordChar = true;
            // 
            // lblClave
            // 
            this.lblClave.AutoSize = true;
            this.lblClave.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblClave.Location = new System.Drawing.Point(21, 184);
            this.lblClave.Name = "lblClave";
            this.lblClave.Size = new System.Drawing.Size(79, 13);
            this.lblClave.TabIndex = 3;
            this.lblClave.Text = "CONTRASEÑA:";
            // 
            // txtUsuario
            // 
            this.txtUsuario.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUsuario.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsuario.Location = new System.Drawing.Point(24, 142);
            this.txtUsuario.MaxLength = 50;
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(382, 26);
            this.txtUsuario.TabIndex = 2;
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsuario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblUsuario.Location = new System.Drawing.Point(21, 122);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(127, 13);
            this.lblUsuario.TabIndex = 1;
            this.lblUsuario.Text = "NOMBRE DE USUARIO:";
            // 
            // pnlCardHeader
            // 
            this.pnlCardHeader.Controls.Add(this.pnlLoginLogo);
            this.pnlCardHeader.Controls.Add(this.lblHeaderSubtitle);
            this.pnlCardHeader.Controls.Add(this.lblNodeBadge);
            this.pnlCardHeader.Controls.Add(this.lblHeaderTitle);
            this.pnlCardHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCardHeader.Location = new System.Drawing.Point(24, 24);
            this.pnlCardHeader.Name = "pnlCardHeader";
            this.pnlCardHeader.Size = new System.Drawing.Size(382, 70);
            this.pnlCardHeader.TabIndex = 0;
            // 
            // pnlLoginLogo
            // 
            this.pnlLoginLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlLoginLogo.Controls.Add(this.lblLoginLogo);
            this.pnlLoginLogo.Location = new System.Drawing.Point(0, 10);
            this.pnlLoginLogo.Name = "pnlLoginLogo";
            this.pnlLoginLogo.Size = new System.Drawing.Size(34, 34);
            this.pnlLoginLogo.TabIndex = 3;
            // 
            // lblLoginLogo
            // 
            this.lblLoginLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLoginLogo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoginLogo.ForeColor = System.Drawing.Color.White;
            this.lblLoginLogo.Location = new System.Drawing.Point(0, 0);
            this.lblLoginLogo.Name = "lblLoginLogo";
            this.lblLoginLogo.Size = new System.Drawing.Size(34, 34);
            this.lblLoginLogo.TabIndex = 0;
            this.lblLoginLogo.Text = "TS";
            this.lblLoginLogo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblHeaderSubtitle
            // 
            this.lblHeaderSubtitle.AutoSize = true;
            this.lblHeaderSubtitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblHeaderSubtitle.Location = new System.Drawing.Point(44, 32);
            this.lblHeaderSubtitle.Name = "lblHeaderSubtitle";
            this.lblHeaderSubtitle.Size = new System.Drawing.Size(262, 15);
            this.lblHeaderSubtitle.TabIndex = 2;
            this.lblHeaderSubtitle.Text = "Terminal Punto de Venta & Facturación Electrónica";
            // 
            // lblNodeBadge
            // 
            this.lblNodeBadge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNodeBadge.AutoSize = true;
            this.lblNodeBadge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.lblNodeBadge.Font = new System.Drawing.Font("Consolas", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNodeBadge.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblNodeBadge.Location = new System.Drawing.Point(315, 12);
            this.lblNodeBadge.Name = "lblNodeBadge";
            this.lblNodeBadge.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.lblNodeBadge.Size = new System.Drawing.Size(67, 16);
            this.lblNodeBadge.TabIndex = 1;
            this.lblNodeBadge.Text = "v2.4-clean";
            // 
            // lblHeaderTitle
            // 
            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 12.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblHeaderTitle.Location = new System.Drawing.Point(42, 8);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(127, 23);
            this.lblHeaderTitle.TabIndex = 0;
            this.lblHeaderTitle.Text = "TechStore POS";
            // 
            // frmLogin
            // 
            this.AcceptButton = this.btnIngresar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(251)))), ((int)(((byte)(253)))));
            this.ClientSize = new System.Drawing.Size(500, 545);
            this.Controls.Add(this.pnlCenterCard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TechStore POS - Autenticación y Control de Acceso";
            this.pnlCenterCard.ResumeLayout(false);
            this.pnlCenterCard.PerformLayout();
            this.grpDemo.ResumeLayout(false);
            this.pnlCardHeader.ResumeLayout(false);
            this.pnlCardHeader.PerformLayout();
            this.pnlLoginLogo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlCenterCard;
        private System.Windows.Forms.Panel pnlCardHeader;
        private System.Windows.Forms.Panel pnlLoginLogo;
        private System.Windows.Forms.Label lblLoginLogo;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Label lblNodeBadge;
        private System.Windows.Forms.Label lblHeaderSubtitle;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label lblClave;
        private System.Windows.Forms.TextBox txtClave;
        private System.Windows.Forms.CheckBox chkMostrarClave;
        private System.Windows.Forms.Button btnIngresar;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.GroupBox grpDemo;
        private System.Windows.Forms.Button btnDemoAdmin;
        private System.Windows.Forms.Button btnDemoCajero;
        private System.Windows.Forms.Button btnDemoCajero2;
        private System.Windows.Forms.Label lblInfoSeguridad;
    }
}
