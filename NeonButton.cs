using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SportPitShop
{
    public class NeonButton : Control
    {
        public Color BorderColor { get; set; } = Ui.Accent2;
        public Color GlowColor { get; set; } = Ui.Accent;
        public Color FillColor { get; set; } = Color.FromArgb(18, 18, 24);
        public bool IsPrimary { get; set; } = false;

        private bool _hover;
        private bool _down;

        public event EventHandler Clicked;

        public NeonButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
            Cursor = Cursors.Hand;
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 10, FontStyle.Bold);
            Size = new Size(110, 34);

            MouseEnter += (s, e) => { _hover = true; Invalidate(); };
            MouseLeave += (s, e) => { _hover = false; _down = false; Invalidate(); };
            MouseDown += (s, e) => { if (e.Button == MouseButtons.Left) { _down = true; Invalidate(); } };
            MouseUp += (s, e) =>
            {
                if (_down && e.Button == MouseButtons.Left)
                {
                    _down = false;
                    Invalidate();
                    Clicked?.Invoke(this, EventArgs.Empty);
                }
            };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            int radius = 14;

            using (var path = RoundedRect(rect, radius))
            {
                Color fill = IsPrimary ? Color.FromArgb(22, 26, 34) : FillColor;
                if (_down) fill = Color.FromArgb(28, 30, 40);

                using (var br = new SolidBrush(fill))
                    g.FillPath(br, path);

                Color border = _hover ? GlowColor : BorderColor;
                float bw = _hover ? 2.2f : 1.4f;

                using (var pen = new Pen(border, bw))
                    g.DrawPath(pen, path);

                if (_hover)
                {
                    using (var penGlow = new Pen(Color.FromArgb(70, GlowColor), 7f))
                        g.DrawPath(penGlow, path);
                }

                TextRenderer.DrawText(g, Text, Font, rect, ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
