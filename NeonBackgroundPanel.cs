using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SportPitShop
{
    // Flicker-safe neon background without BackgroundImage
    public class NeonBackgroundPanel : Panel
    {
        public NeonBackgroundPanel()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var b = new LinearGradientBrush(this.ClientRectangle,
                Color.FromArgb(10, 10, 14),
                Color.FromArgb(20, 22, 30),
                90f))
            {
                g.FillRectangle(b, this.ClientRectangle);
            }

            // subtle vignette
            using (var path = new GraphicsPath())
            {
                path.AddEllipse(-this.Width / 3, -this.Height / 3, this.Width + this.Width * 2 / 3, this.Height + this.Height * 2 / 3);
                using (var pgb = new PathGradientBrush(path))
                {
                    pgb.CenterColor = Color.FromArgb(50, 74, 126, 255);
                    pgb.SurroundColors = new[] { Color.FromArgb(0, 0, 0, 0) };
                    g.FillRectangle(pgb, this.ClientRectangle);
                }
            }

            // neon top line
            using (var pen = new Pen(Color.FromArgb(140, 74, 126, 255), 3f))
                g.DrawLine(pen, 0, 0, this.Width, 0);

            using (var pen2 = new Pen(Color.FromArgb(80, 120, 201, 255), 1f))
                g.DrawLine(pen2, 0, 3, this.Width, 3);
        }
    }
}
