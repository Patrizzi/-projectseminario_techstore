using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TechStore.UI
{
    /// <summary>
    /// Sistema de diseño "Precision Retail Workspace" (DESIGN.md).
    /// Centraliza paleta, tipografía y estilos de controles.
    /// </summary>
    public static class Tema
    {
        // Paleta
        public static readonly Color Canvas = Color.FromArgb(248, 250, 252);
        public static readonly Color Surface = Color.White;
        public static readonly Color Sunken = Color.FromArgb(241, 245, 249);
        public static readonly Color Border = Color.FromArgb(226, 232, 240);
        public static readonly Color BorderStrong = Color.FromArgb(203, 213, 225);
        public static readonly Color TextPrimary = Color.FromArgb(15, 23, 42);
        public static readonly Color TextSecondary = Color.FromArgb(71, 85, 105);
        public static readonly Color Primary = Color.FromArgb(59, 91, 245);
        public static readonly Color Emerald = Color.FromArgb(5, 150, 105);
        public static readonly Color Amber = Color.FromArgb(245, 158, 11);
        public static readonly Color Error = Color.FromArgb(186, 26, 26);

        private static readonly string FuenteUi = Disponible("Inter") ? "Inter" : "Segoe UI";
        private static readonly string FuenteMono = Disponible("JetBrains Mono") ? "JetBrains Mono" : "Consolas";

        private static bool Disponible(string familia)
        {
            using (var f = new Font(familia, 9F))
                return string.Equals(f.Name, familia, StringComparison.OrdinalIgnoreCase);
        }

        public static Font Ui(float size, FontStyle style = FontStyle.Regular) { return new Font(FuenteUi, size, style); }
        public static Font Mono(float size, FontStyle style = FontStyle.Regular) { return new Font(FuenteMono, size, style); }

        // Reemplaza los tonos azulados antiguos por la paleta slate/indigo/emerald.
        private static Color Mapear(Color c)
        {
            int k = (c.R << 16) | (c.G << 8) | c.B;
            switch (k)
            {
                case (248 << 16) | (249 << 8) | 255: return Canvas;
                case (239 << 16) | (244 << 8) | 255: return Sunken;
                case (220 << 16) | (233 << 8) | 255: return Border;
                case (229 << 16) | (238 << 8) | 255: return Border;
                case (68 << 16) | (70 << 8) | 85: return TextSecondary;
                case (11 << 16) | (28 << 8) | 48: return TextPrimary;
                case (22 << 16) | (62 << 8) | 220: return Primary;
                case (0 << 16) | (48 << 8) | 199: return Primary;
                case (0 << 16) | (108 << 8) | 74: return Emerald;
                case (0 << 16) | (168 << 8) | 107: return Color.FromArgb(16, 185, 129);
                default: return c;
            }
        }

        /// <summary>Aplica tipografía y paleta recursivamente.</summary>
        public static void Aplicar(Control raiz)
        {
            foreach (Control c in raiz.Controls)
            {
                if (!(c is Form))
                {
                    Color bg = Mapear(c.BackColor);
                    if (bg != c.BackColor) c.BackColor = bg;
                    Color fg = Mapear(c.ForeColor);
                    if (fg != c.ForeColor) c.ForeColor = fg;

                    string fam = c.Font.FontFamily.Name;
                    if (fam == "Segoe UI") c.Font = new Font(FuenteUi, c.Font.Size, c.Font.Style);
                    else if (fam == "Consolas") c.Font = new Font(FuenteMono, c.Font.Size, c.Font.Style);
                }

                var btn = c as Button;
                if (btn != null && btn.FlatStyle == FlatStyle.Flat)
                {
                    Color borde = Mapear(btn.FlatAppearance.BorderColor);
                    btn.FlatAppearance.BorderColor = borde == Color.Empty || borde.A == 0 ? BorderStrong : borde;
                }

                var dgv = c as DataGridView;
                if (dgv != null) EstilizarGrilla(dgv);

                if (c.HasChildren) Aplicar(c);
            }
        }

        public static void EstilizarGrilla(DataGridView g)
        {
            g.BackgroundColor = Surface;
            g.BorderStyle = BorderStyle.None;
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            g.GridColor = Border;
            g.EnableHeadersVisualStyles = false;
            g.RowHeadersVisible = false;
            g.RowTemplate.Height = 36;
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            g.ColumnHeadersHeight = 34;
            g.ColumnHeadersDefaultCellStyle.BackColor = Surface;
            g.ColumnHeadersDefaultCellStyle.ForeColor = TextSecondary;
            g.ColumnHeadersDefaultCellStyle.SelectionBackColor = Surface;
            g.ColumnHeadersDefaultCellStyle.SelectionForeColor = TextSecondary;
            g.ColumnHeadersDefaultCellStyle.Font = Mono(8F, FontStyle.Bold);
            g.DefaultCellStyle.BackColor = Surface;
            g.DefaultCellStyle.ForeColor = TextPrimary;
            g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(238, 242, 255);
            g.DefaultCellStyle.SelectionForeColor = TextPrimary;
            g.AlternatingRowsDefaultCellStyle.BackColor = Surface;
        }

        /// <summary>Pestañas planas tipo "pill" como en el mockup.</summary>
        public static void EstilizarTabs(TabControl tabs)
        {
            tabs.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabs.SizeMode = TabSizeMode.Fixed;
            tabs.ItemSize = new Size(190, 34);
            tabs.Padding = new Point(12, 4);
            tabs.DrawItem += (s, e) =>
            {
                var t = (TabControl)s;
                bool sel = e.Index == t.SelectedIndex;
                Rectangle r = Rectangle.Inflate(t.GetTabRect(e.Index), -4, -2);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var fondo = new SolidBrush(Canvas))
                    e.Graphics.FillRectangle(fondo, t.GetTabRect(e.Index));
                if (sel)
                    using (var b = new SolidBrush(Sunken))
                    using (var path = Redondeado(r, 6))
                        e.Graphics.FillPath(b, path);
                TextRenderer.DrawText(e.Graphics, t.TabPages[e.Index].Text, Ui(9F, sel ? FontStyle.Bold : FontStyle.Regular),
                    r, sel ? TextPrimary : TextSecondary,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            };
        }

        public static GraphicsPath Redondeado(Rectangle r, int radio)
        {
            int d = radio * 2;
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }
    }
}

