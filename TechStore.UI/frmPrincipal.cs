using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TechStore.BLL;
using TechStore.Entidades;

namespace TechStore.UI
{
    public partial class frmPrincipal : Form
    {
        // Capa de Lógica de Negocio (BLL)
        private readonly ProductoBLL _productoBLL = new ProductoBLL();
        private readonly VentaBLL _ventaBLL = new VentaBLL();
        private readonly UsuarioBLL _usuarioBLL = new UsuarioBLL();

        // Carrito de compras en memoria
        private readonly BindingList<DetalleVenta> _listaDetalles = new BindingList<DetalleVenta>();

        // Catálogo de productos en memoria
        private List<Producto> _catalogoProductos = new List<Producto>();
        private Producto _productoSeleccionado = null;

        // Tipo de comprobante activo ('FACTURA', 'BOLETA', 'NOTA')
        private string _tipoComprobanteActivo = "FACTURA";

        // Método de pago activo ('EFECTIVO', 'TARJETA', 'YAPE')
        private string _metodoPagoActivo = "EFECTIVO";

        public frmPrincipal()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            // 1. Mostrar información del usuario desde el Singleton (Thread-Safe)
            MostrarInformacionUsuarioSesion();

            // 2. Configurar cuadrícula de carrito
            ConfigurarGrillaDetalleVenta();

            // 3. Cargar catálogo, inventario e historial
            CargarCatalogoProductos();
            CargarInventario();
            CargarHistorialVentas();

            // 4. Inicializar totales
            ActualizarTotales();
        }

        /// <summary>
        /// Lee la instancia única del Singleton SesionUsuario para la UI.
        /// </summary>
        private void MostrarInformacionUsuarioSesion()
        {
            if (SesionUsuario.Instancia.HaySesionActiva)
            {
                lblCashierName.Text = SesionUsuario.Instancia.NombreCompleto;
                lblCashierSub.Text = string.Format("{0} (@{1}) - Caja #02",
                    SesionUsuario.Instancia.Rol,
                    SesionUsuario.Instancia.NombreUsuario);
            }
            else
            {
                lblCashierName.Text = "Sesión No Autenticada";
                lblCashierSub.Text = "Terminal Desconectado";
            }
        }

        #region Diseño (Precision Retail Workspace)

        private void AplicarDiseno()
        {
            this.SuspendLayout();
            this.BackColor = Tema.Canvas;
            this.Font = Tema.Ui(9F);
            Tema.Aplicar(this);
            Tema.EstilizarTabs(tabMain);

            // Título y cabecera estilo Apple / Vercel Ultra-clean
            lblBrandTitle.Text = "TechStore POS";
            lblBrandTitle.Font = Tema.Ui(13F, FontStyle.Bold);
            lblVersion.Text = "v2.4-clean";
            lblSystemSub.Text = "Terminal T-LIMA-02 • Serie F002";
            btnCerrarSesion.BackColor = Tema.Surface;
            btnCerrarSesion.ForeColor = Tema.TextSecondary;
            btnCerrarSesion.FlatAppearance.BorderColor = Tema.Border;
            btnCerrarSesion.Text = "Salir";

            // Contenedores
            tabVenta.BackColor = Tema.Canvas;
            tabInventario.BackColor = Tema.Canvas;
            tabHistorial.BackColor = Tema.Canvas;
            pnlTopHeader.BackColor = Tema.Surface;

            // Tarjetas numeradas
            lblCard1Title.Text = "1. Búsqueda y Selección de Producto";
            lblCard2Title.Text = "2. Carrito de Compras";
            lblCard3Title.Text = "3. Facturación";

            // CTA principal: Agregar (oscuro) y Facturar (esmeralda con brillo)
            btnAgregar.BackColor = Tema.TextPrimary;
            btnAgregar.Text = "+  Agregar al Carrito";
            btnRegistrarVenta.BackColor = Tema.Emerald;
            btnRegistrarVenta.ForeColor = Color.White;
            btnRegistrarVenta.FlatAppearance.BorderSize = 0;
            btnRegistrarVenta.FlatAppearance.MouseDownBackColor = Color.FromArgb(4, 120, 87);
            btnRegistrarVenta.FlatAppearance.MouseOverBackColor = Color.FromArgb(4, 135, 97);

            // Total ledger oscuro con cifra monoespaciada grande
            pnlLedgerTotalCard.BackColor = Tema.TextPrimary;
            lblTotalTitulo.BackColor = Tema.TextPrimary;
            lblTotalTitulo.ForeColor = Color.FromArgb(148, 163, 184);
            lblTotalCurrency.BackColor = Tema.TextPrimary;
            lblTotalCurrency.ForeColor = Color.FromArgb(148, 163, 184);
            lblTotalValor.BackColor = Tema.TextPrimary;
            lblTotalValor.ForeColor = Color.White;
            lblTotalValor.Font = Tema.Mono(24F, FontStyle.Bold);

            // Estado inicial de selectores (Factura / Efectivo)
            pnlVoucherTabs.BackColor = Tema.Sunken;
            btnVoucher_Click(btnVoucherFactura, EventArgs.Empty);
            btnPayMethod_Click(btnPayCash, EventArgs.Empty);

            this.ResumeLayout(true);
        }

        #endregion
        #region Atajos de Teclado del POS ([F2], [F4], [F12])

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F2)
            {
                txtBuscarProducto.Focus();
                txtBuscarProducto.SelectAll();
                return true;
            }
            if (keyData == Keys.F4)
            {
                btnLimpiar.PerformClick();
                return true;
            }
            if (keyData == Keys.F12)
            {
                btnRegistrarVenta.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        #region Configuración y Carga de Catálogo

        private void ConfigurarGrillaDetalleVenta()
        {
            dgvDetalles.AutoGenerateColumns = false;
            dgvDetalles.Columns.Clear();

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdProducto",
                HeaderText = "ID",
                Visible = false
            });

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CodigoProducto",
                HeaderText = "CÓDIGO",
                FillWeight = 40,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = Tema.Mono(8.5F, FontStyle.Bold),
                    ForeColor = Tema.Primary
                }
            });

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreProducto",
                HeaderText = "DESCRIPCIÓN DEL PRODUCTO",
                FillWeight = 140,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = Tema.Ui(9F, FontStyle.Regular),
                    ForeColor = Tema.TextPrimary
                }
            });

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Cantidad",
                HeaderText = "CANTIDAD",
                FillWeight = 35,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = Tema.Mono(9F, FontStyle.Bold)
                }
            });

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PrecioUnitario",
                HeaderText = "PRECIO UNIT. (S/)",
                FillWeight = 45,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "N2",
                    Font = Tema.Mono(9F, FontStyle.Regular),
                    ForeColor = Tema.TextSecondary
                }
            });

            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Subtotal",
                HeaderText = "SUBTOTAL (S/)",
                FillWeight = 45,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "N2",
                    Font = Tema.Mono(9F, FontStyle.Bold),
                    ForeColor = Tema.TextPrimary
                }
            });

            dgvDetalles.DataSource = _listaDetalles;
        }

        private void CargarCatalogoProductos()
        {
            try
            {
                _catalogoProductos = _productoBLL.ListarProductos(true);

                cboProductos.SelectedIndexChanged -= cboProductos_SelectedIndexChanged;
                cboProductos.DataSource = null;
                cboProductos.DataSource = _catalogoProductos;
                cboProductos.DisplayMember = "Nombre";
                cboProductos.ValueMember = "IdProducto";
                cboProductos.SelectedIndex = -1;
                cboProductos.SelectedIndexChanged += cboProductos_SelectedIndexChanged;

                LimpiarCamposProducto();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar el catálogo de productos: " + ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LimpiarCamposProducto()
        {
            _productoSeleccionado = null;
            lblMetricSkuVal.Text = "--";
            lblMetricCatVal.Text = "--";
            lblMetricPriceVal.Text = "S/ 0.00";
            lblMetricStockVal.Text = "--";
            lblMetricStockVal.ForeColor = Tema.TextPrimary;
            nudCantidad.Value = 1;
            lblMaxStockHint.Text = "(Máx. en tienda)";
        }

        private void cboProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProductos.SelectedItem is Producto prod)
            {
                SeleccionarProducto(prod);
            }
            else
            {
                LimpiarCamposProducto();
            }
        }

        private void txtBuscarProducto_TextChanged(object sender, EventArgs e)
        {
            string query = txtBuscarProducto.Text.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(query))
                return;

            Producto match = _catalogoProductos.FirstOrDefault(p =>
                p.Codigo.ToLower().Contains(query) ||
                p.Nombre.ToLower().Contains(query)
            );

            if (match != null)
            {
                cboProductos.SelectedItem = match;
            }
        }

        private void SeleccionarProducto(Producto prod)
        {
            _productoSeleccionado = prod;
            lblMetricSkuVal.Text = prod.Codigo;
            lblMetricCatVal.Text = prod.NombreCategoria ?? "General";
            lblMetricPriceVal.Text = string.Format("S/ {0:N2}", prod.Precio);
            lblMetricStockVal.Text = string.Format("{0} unid.", prod.Stock);

            // Código de color del stock según el design system
            if (prod.Stock <= 0)
            {
                lblMetricStockVal.ForeColor = Tema.Error; // Error
                lblMaxStockHint.Text = "(Agotado)";
                nudCantidad.Maximum = 1;
            }
            else if (prod.Stock <= 3)
            {
                lblMetricStockVal.ForeColor = Tema.Amber; // Amber warning
                lblMaxStockHint.Text = string.Format("(Stock bajo: máx. {0})", prod.Stock);
                nudCantidad.Maximum = prod.Stock;
            }
            else
            {
                lblMetricStockVal.ForeColor = Tema.Emerald; // Ledger emerald
                lblMaxStockHint.Text = string.Format("(Máx. {0} unid.)", prod.Stock);
                nudCantidad.Maximum = prod.Stock;
            }

            nudCantidad.Value = 1;
        }

        #endregion

        #region Carrito de Compras (Líneas de Detalle)

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_productoSeleccionado == null)
                {
                    MessageBox.Show("Seleccione un producto del catálogo o escanee el código para continuar.",
                                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboProductos.Focus();
                    return;
                }

                int cantidadSolicitada = (int)nudCantidad.Value;

                if (cantidadSolicitada <= 0)
                {
                    MessageBox.Show("La cantidad a añadir debe ser mayor a cero (0).",
                                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    nudCantidad.Focus();
                    return;
                }

                if (_productoSeleccionado.Stock <= 0)
                {
                    MessageBox.Show(string.Format("El producto '{0}' no cuenta con unidades disponibles.", _productoSeleccionado.Nombre),
                                    "Sin Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DetalleVenta itemExistente = _listaDetalles.FirstOrDefault(d => d.IdProducto == _productoSeleccionado.IdProducto);
                int cantidadPrevia = itemExistente != null ? itemExistente.Cantidad : 0;
                int totalRequerido = cantidadPrevia + cantidadSolicitada;

                if (totalRequerido > _productoSeleccionado.Stock)
                {
                    MessageBox.Show(
                        string.Format("No es posible añadir {0} unidad(es).\nDisponible en tienda: {1} | Ya en carrito: {2}.",
                        cantidadSolicitada, _productoSeleccionado.Stock, cantidadPrevia),
                        "Stock Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (itemExistente != null)
                {
                    itemExistente.Cantidad = totalRequerido;
                    itemExistente.CalcularSubtotal();
                    dgvDetalles.Refresh();
                }
                else
                {
                    DetalleVenta nuevoDetalle = new DetalleVenta
                    {
                        IdProducto = _productoSeleccionado.IdProducto,
                        CodigoProducto = _productoSeleccionado.Codigo,
                        NombreProducto = _productoSeleccionado.Nombre,
                        Cantidad = cantidadSolicitada,
                        PrecioUnitario = _productoSeleccionado.Precio
                    };
                    nuevoDetalle.CalcularSubtotal();
                    _listaDetalles.Add(nuevoDetalle);
                }

                ActualizarTotales();

                cboProductos.SelectedIndex = -1;
                txtBuscarProducto.Clear();
                txtBuscarProducto.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (dgvDetalles.CurrentRow != null && dgvDetalles.CurrentRow.DataBoundItem is DetalleVenta item)
            {
                _listaDetalles.Remove(item);
                ActualizarTotales();
            }
            else
            {
                MessageBox.Show("Seleccione una fila del carrito para remover.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            if (_listaDetalles.Count == 0) return;

            DialogResult dr = MessageBox.Show("¿Está seguro de vaciar todos los artículos del carrito actual?",
                                              "Vaciar Carrito [F4]", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                _listaDetalles.Clear();
                ActualizarTotales();
            }
        }

        private void ActualizarTotales()
        {
            int totalItems = _listaDetalles.Sum(d => d.Cantidad);
            decimal totalPagar = _listaDetalles.Sum(d => d.Subtotal);

            decimal subtotal = Math.Round(totalPagar / 1.18m, 2);
            decimal igv = totalPagar - subtotal;

            lblItemsValor.Text = string.Format("{0} artículos ({1} líneas)", totalItems, _listaDetalles.Count);
            lblSubtotalValor.Text = string.Format("S/ {0:N2}", subtotal);
            lblIGVValor.Text = string.Format("S/ {0:N2}", igv);
            lblTotalValor.Text = string.Format("{0:N2}", totalPagar);
            lblCartLinesBadge.Text = string.Format("{0} líneas activas", _listaDetalles.Count);

            // Sincronizar monto recibido por defecto
            txtCashReceived.Text = totalPagar.ToString("N2");
            CalcularVuelto();
        }

        private void txtCashReceived_TextChanged(object sender, EventArgs e)
        {
            CalcularVuelto();
        }

        private void CalcularVuelto()
        {
            decimal totalPagar = _listaDetalles.Sum(d => d.Subtotal);
            decimal recibido;
            if (decimal.TryParse(txtCashReceived.Text.Trim(), out recibido))
            {
                decimal vuelto = recibido - totalPagar;
                if (vuelto < 0)
                {
                    lblChangeVal.Text = "S/ 0.00";
                    lblChangeVal.ForeColor = Tema.Error;
                }
                else
                {
                    lblChangeVal.Text = string.Format("S/ {0:N2}", vuelto);
                    lblChangeVal.ForeColor = Tema.Emerald;
                }
            }
            else
            {
                lblChangeVal.Text = "S/ 0.00";
            }
        }

        #endregion

        #region Comprobantes y Métodos de Pago

        private void btnVoucher_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            // Restablecer estilos de los 3 botones
            btnVoucherFactura.BackColor = Tema.Sunken;
            btnVoucherFactura.ForeColor = Tema.TextSecondary;
            btnVoucherFactura.Font = Tema.Ui(8.5F, FontStyle.Regular);

            btnVoucherBoleta.BackColor = Tema.Sunken;
            btnVoucherBoleta.ForeColor = Tema.TextSecondary;
            btnVoucherBoleta.Font = Tema.Ui(8.5F, FontStyle.Regular);

            btnVoucherNota.BackColor = Tema.Sunken;
            btnVoucherNota.ForeColor = Tema.TextSecondary;
            btnVoucherNota.Font = Tema.Ui(8.5F, FontStyle.Regular);

            // Activar botón seleccionado
            btn.BackColor = Tema.Primary;
            btn.ForeColor = Color.White;
            btn.Font = Tema.Ui(8.5F, FontStyle.Bold);

            if (btn == btnVoucherFactura)
            {
                _tipoComprobanteActivo = "FACTURA";
                txtCustomerRuc.Text = "20601234567";
                txtCustomerName.Text = "TECH SOLUTIONS PERU S.A.C.";
                lblCustomerBadge.Text = "SUNAT RUC ACTIVO";
            }
            else if (btn == btnVoucherBoleta)
            {
                _tipoComprobanteActivo = "BOLETA";
                txtCustomerRuc.Text = "73428901";
                txtCustomerName.Text = "CARLOS RAMOS FLORES";
                lblCustomerBadge.Text = "RENIEC DNI VÁLIDO";
            }
            else
            {
                _tipoComprobanteActivo = "NOTA";
                txtCustomerRuc.Text = "00000000";
                txtCustomerName.Text = "CLIENTE FINAL (VENTA RÁPIDA)";
                lblCustomerBadge.Text = "INTERNO";
            }
        }

        private void btnPayMethod_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            btnPayCash.BackColor = Tema.Surface;
            btnPayCash.ForeColor = Tema.TextSecondary;

            btnPayCard.BackColor = Tema.Surface;
            btnPayCard.ForeColor = Tema.TextSecondary;

            btnPayYape.BackColor = Tema.Surface;
            btnPayYape.ForeColor = Tema.TextSecondary;

            btn.BackColor = Tema.Border;
            btn.ForeColor = Tema.Primary;

            if (btn == btnPayCash) _metodoPagoActivo = "EFECTIVO";
            else if (btn == btnPayCard) _metodoPagoActivo = "TARJETA";
            else _metodoPagoActivo = "YAPE";
        }

        #endregion

        #region Registro de Venta y Transacción Atómica (SqlTransaction)

        private void btnRegistrarVenta_Click(object sender, EventArgs e)
        {
            try
            {
                // Validación 1: No registrar ventas vacías (Rúbrica)
                if (_listaDetalles.Count == 0)
                {
                    MessageBox.Show("No se puede registrar una venta sin productos.\nAñada al menos un producto al carrito de compras.",
                                    "Validación de Venta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtBuscarProducto.Focus();
                    return;
                }

                decimal totalVenta = _listaDetalles.Sum(d => d.Subtotal);

                // Confirmación
                string mensaje = string.Format(
                    "¿Desea confirmar y emitir este comprobante?\n\n" +
                    "• Tipo: {0}\n" +
                    "• Cliente: {1} ({2})\n" +
                    "• Total: S/. {3:N2}\n" +
                    "• Método de Pago: {4}\n" +
                    "• Cajero: {5}\n\n" +
                    "Se ejecutará la transacción atómica (SqlTransaction) y el descuento de inventario.",
                    _tipoComprobanteActivo,
                    txtCustomerName.Text,
                    txtCustomerRuc.Text,
                    totalVenta,
                    _metodoPagoActivo,
                    SesionUsuario.Instancia.NombreCompleto);

                DialogResult dr = MessageBox.Show(mensaje, "Confirmar Factura [F12]", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr != DialogResult.Yes)
                    return;

                // Crear Cabecera de Venta
                Venta nuevaVenta = new Venta
                {
                    Fecha = DateTime.Now,
                    Total = totalVenta,
                    IdUsuario = SesionUsuario.Instancia.IdUsuario,
                    IdProducto = _listaDetalles.Count > 0 ? (int?)_listaDetalles[0].IdProducto : null,
                    Estado = "COMPLETADA"
                };

                List<DetalleVenta> detallesParaGuardar = _listaDetalles.ToList();

                // Ejecución Transaccional en BLL -> DAL con SqlTransaction
                int idVentaGenerada = _ventaBLL.RegistrarVenta(nuevaVenta, detallesParaGuardar);

                string mensajeExito = string.Format(
                    "¡COMPROBANTE REGISTRADO CON ÉXITO!\n\n" +
                    "• N° Transacción / Venta: {0}\n" +
                    "• Comprobante: {1} F002-{0:D6}\n" +
                    "• Cajero: {2}\n" +
                    "• Total Facturado: S/. {3:N2}\n\n" +
                    "✔ Transacción atómica completada (Cero datos huérfanos).\n" +
                    "✔ Stock de productos actualizado en inventario.",
                    idVentaGenerada,
                    _tipoComprobanteActivo,
                    SesionUsuario.Instancia.NombreCompleto,
                    totalVenta);

                MessageBox.Show(mensajeExito, "Venta Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpiar carrito y campos
                _listaDetalles.Clear();
                ActualizarTotales();
                LimpiarCamposProducto();

                // Recargar catálogo e inventario
                CargarCatalogoProductos();
                CargarInventario();
                CargarHistorialVentas();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Validación de Transacción", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "La operación de venta no pudo completarse y fue cancelada (Rollback total).\n" +
                    "Ningún dato huérfano fue almacenado en la base de datos.\n\n" +
                    "Detalle: " + ex.Message,
                    "Transacción Abortada", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Inventario en Tiempo Real

        private void CargarInventario()
        {
            try
            {
                List<Producto> lista = _productoBLL.ListarProductos(false);
                dgvInventario.DataSource = lista;
                FormatearGrillaInventario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar el inventario: " + ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FormatearGrillaInventario()
        {
            if (dgvInventario.Columns.Count == 0) return;

            if (dgvInventario.Columns["IdProducto"] != null) dgvInventario.Columns["IdProducto"].HeaderText = "ID";
            if (dgvInventario.Columns["Codigo"] != null)
            {
                dgvInventario.Columns["Codigo"].HeaderText = "CÓDIGO / SKU";
                dgvInventario.Columns["Codigo"].DefaultCellStyle.Font = Tema.Mono(8.5F, FontStyle.Bold);
                dgvInventario.Columns["Codigo"].DefaultCellStyle.ForeColor = Tema.Primary;
            }
            if (dgvInventario.Columns["Nombre"] != null) dgvInventario.Columns["Nombre"].HeaderText = "PRODUCTO";
            if (dgvInventario.Columns["NombreCategoria"] != null) dgvInventario.Columns["NombreCategoria"].HeaderText = "CATEGORÍA";
            if (dgvInventario.Columns["Precio"] != null)
            {
                dgvInventario.Columns["Precio"].HeaderText = "PRECIO (S/)";
                dgvInventario.Columns["Precio"].DefaultCellStyle.Format = "N2";
                dgvInventario.Columns["Precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvInventario.Columns["Precio"].DefaultCellStyle.Font = Tema.Mono(9F, FontStyle.Bold);
            }
            if (dgvInventario.Columns["Stock"] != null)
            {
                dgvInventario.Columns["Stock"].HeaderText = "STOCK ACTUAL";
                dgvInventario.Columns["Stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvInventario.Columns["Stock"].DefaultCellStyle.Font = Tema.Mono(9.5F, FontStyle.Bold);
            }

            if (dgvInventario.Columns["Descripcion"] != null) dgvInventario.Columns["Descripcion"].Visible = false;
            if (dgvInventario.Columns["IdCategoria"] != null) dgvInventario.Columns["IdCategoria"].Visible = false;
            if (dgvInventario.Columns["Estado"] != null) dgvInventario.Columns["Estado"].Visible = false;
        }

        private void txtBuscarInventario_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtBuscarInventario.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(filtro))
            {
                dgvInventario.DataSource = _catalogoProductos;
            }
            else
            {
                var filtrados = _catalogoProductos.Where(p =>
                    p.Nombre.ToLower().Contains(filtro) ||
                    p.Codigo.ToLower().Contains(filtro) ||
                    (p.NombreCategoria != null && p.NombreCategoria.ToLower().Contains(filtro))
                ).ToList();

                dgvInventario.DataSource = filtrados;
            }
            FormatearGrillaInventario();
        }

        private void btnRefrescarInventario_Click(object sender, EventArgs e)
        {
            CargarInventario();
            CargarCatalogoProductos();
        }

        #endregion

        #region Historial y Auditoría Transaccional

        private void CargarHistorialVentas()
        {
            try
            {
                List<Venta> lista = _ventaBLL.ListarVentas();
                dgvHistorialVentas.DataSource = lista;
                FormatearGrillaHistorialVentas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar el historial de ventas: " + ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FormatearGrillaHistorialVentas()
        {
            if (dgvHistorialVentas.Columns.Count == 0) return;

            if (dgvHistorialVentas.Columns["IdVenta"] != null) dgvHistorialVentas.Columns["IdVenta"].HeaderText = "ID VENTA";
            if (dgvHistorialVentas.Columns["NumeroFactura"] != null)
            {
                dgvHistorialVentas.Columns["NumeroFactura"].HeaderText = "N° COMPROBANTE";
                dgvHistorialVentas.Columns["NumeroFactura"].DefaultCellStyle.Font = Tema.Mono(8.5F, FontStyle.Bold);
                dgvHistorialVentas.Columns["NumeroFactura"].DefaultCellStyle.ForeColor = Tema.Primary;
            }
            if (dgvHistorialVentas.Columns["Fecha"] != null)
            {
                dgvHistorialVentas.Columns["Fecha"].HeaderText = "FECHA Y HORA";
                dgvHistorialVentas.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
            }
            if (dgvHistorialVentas.Columns["Cajero"] != null) dgvHistorialVentas.Columns["Cajero"].HeaderText = "CAJERO RESPONSABLE";
            if (dgvHistorialVentas.Columns["CantidadItems"] != null)
            {
                dgvHistorialVentas.Columns["CantidadItems"].HeaderText = "CANT. ÍTEMS";
                dgvHistorialVentas.Columns["CantidadItems"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvHistorialVentas.Columns["Total"] != null)
            {
                dgvHistorialVentas.Columns["Total"].HeaderText = "TOTAL (S/)";
                dgvHistorialVentas.Columns["Total"].DefaultCellStyle.Format = "N2";
                dgvHistorialVentas.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvHistorialVentas.Columns["Total"].DefaultCellStyle.Font = Tema.Mono(9.5F, FontStyle.Bold);
                dgvHistorialVentas.Columns["Total"].DefaultCellStyle.ForeColor = Tema.Emerald;
            }
            if (dgvHistorialVentas.Columns["Estado"] != null) dgvHistorialVentas.Columns["Estado"].HeaderText = "ESTADO";

            if (dgvHistorialVentas.Columns["IdUsuario"] != null) dgvHistorialVentas.Columns["IdUsuario"].Visible = false;
            if (dgvHistorialVentas.Columns["IdProducto"] != null) dgvHistorialVentas.Columns["IdProducto"].Visible = false;
            if (dgvHistorialVentas.Columns["NombreUsuario"] != null) dgvHistorialVentas.Columns["NombreUsuario"].Visible = false;
        }

        private void dgvHistorialVentas_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvHistorialVentas.CurrentRow != null && dgvHistorialVentas.CurrentRow.DataBoundItem is Venta venta)
            {
                try
                {
                    List<DetalleVenta> detalles = _ventaBLL.ObtenerDetallesPorVenta(venta.IdVenta);
                    dgvHistorialDetalles.DataSource = detalles;
                    FormatearGrillaHistorialDetalles();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar detalles de la venta: " + ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                dgvHistorialDetalles.DataSource = null;
            }
        }

        private void FormatearGrillaHistorialDetalles()
        {
            if (dgvHistorialDetalles.Columns.Count == 0) return;

            if (dgvHistorialDetalles.Columns["IdDetalle"] != null) dgvHistorialDetalles.Columns["IdDetalle"].HeaderText = "ID DETALLE";
            if (dgvHistorialDetalles.Columns["CodigoProducto"] != null)
            {
                dgvHistorialDetalles.Columns["CodigoProducto"].HeaderText = "CÓDIGO";
                dgvHistorialDetalles.Columns["CodigoProducto"].DefaultCellStyle.Font = Tema.Mono(8.5F, FontStyle.Bold);
                dgvHistorialDetalles.Columns["CodigoProducto"].DefaultCellStyle.ForeColor = Tema.Primary;
            }
            if (dgvHistorialDetalles.Columns["NombreProducto"] != null) dgvHistorialDetalles.Columns["NombreProducto"].HeaderText = "PRODUCTO";
            if (dgvHistorialDetalles.Columns["Cantidad"] != null)
            {
                dgvHistorialDetalles.Columns["Cantidad"].HeaderText = "CANTIDAD";
                dgvHistorialDetalles.Columns["Cantidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvHistorialDetalles.Columns["PrecioUnitario"] != null)
            {
                dgvHistorialDetalles.Columns["PrecioUnitario"].HeaderText = "PRECIO UNIT. (S/)";
                dgvHistorialDetalles.Columns["PrecioUnitario"].DefaultCellStyle.Format = "N2";
                dgvHistorialDetalles.Columns["PrecioUnitario"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (dgvHistorialDetalles.Columns["Subtotal"] != null)
            {
                dgvHistorialDetalles.Columns["Subtotal"].HeaderText = "SUBTOTAL (S/)";
                dgvHistorialDetalles.Columns["Subtotal"].DefaultCellStyle.Format = "N2";
                dgvHistorialDetalles.Columns["Subtotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvHistorialDetalles.Columns["Subtotal"].DefaultCellStyle.Font = Tema.Mono(9F, FontStyle.Bold);
            }

            if (dgvHistorialDetalles.Columns["IdVenta"] != null) dgvHistorialDetalles.Columns["IdVenta"].Visible = false;
            if (dgvHistorialDetalles.Columns["IdProducto"] != null) dgvHistorialDetalles.Columns["IdProducto"].Visible = false;
        }

        private void btnRefrescarHistorial_Click(object sender, EventArgs e)
        {
            CargarHistorialVentas();
        }

        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabMain.SelectedTab == tabInventario)
            {
                CargarInventario();
            }
            else if (tabMain.SelectedTab == tabHistorial)
            {
                CargarHistorialVentas();
            }
        }

        #endregion

        #region Sesión y Cierre

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("¿Desea cerrar la sesión del terminal actual?",
                                              "Cerrar Sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                _usuarioBLL.CerrarSesion();
                this.Close();
            }
        }

        #endregion

        #region Pintado y Bordes Quirúrgicos (Precision Retail Style)

        private void pnlTopHeader_Paint(object sender, PaintEventArgs e)
        {
            using (Pen p = new Pen(Tema.Border, 1))
            {
                e.Graphics.DrawLine(p, 0, pnlTopHeader.Height - 1, pnlTopHeader.Width, pnlTopHeader.Height - 1);
            }
        }

        private void pnlAvatar_Paint(object sender, PaintEventArgs e)
        {
            Panel pnl = sender as Panel;
            if (pnl == null) return;
            using (Pen p = new Pen(Color.FromArgb(226, 232, 240), 1))
            {
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            }
        }

        private void pnlCard_Paint(object sender, PaintEventArgs e)
        {
            Panel pnl = sender as Panel;
            if (pnl == null) return;
            using (Pen p = new Pen(Color.FromArgb(226, 232, 240), 1))
            {
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            }
        }

        private void pnlSubCard_Paint(object sender, PaintEventArgs e)
        {
            Panel pnl = sender as Panel;
            if (pnl == null) return;
            using (Pen p = new Pen(Color.FromArgb(226, 232, 240), 1))
            {
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            }
        }

        private void pnlLedgerTotalCard_Paint(object sender, PaintEventArgs e)
        {
            using (Pen p = new Pen(Tema.TextPrimary, 1))
            {
                e.Graphics.DrawRectangle(p, 0, 0, pnlLedgerTotalCard.Width - 1, pnlLedgerTotalCard.Height - 1);
            }
        }

        #endregion
    }
}
