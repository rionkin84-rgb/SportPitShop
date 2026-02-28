using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SportPitShop
{
    public class GradientBackgroundPanel : Panel
    {
        private Image _bg;

        public GradientBackgroundPanel()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            LoadBg();
        }

        private void LoadBg()
        {
            try
            {
                var p = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "background.jpg");
                if (File.Exists(p))
                {
                    _bg?.Dispose();
                    _bg = Image.FromFile(p);
                }
            }
            catch { }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (_bg != null)
            {
                e.Graphics.DrawImage(_bg, this.ClientRectangle);
                return;
            }
            base.OnPaintBackground(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _bg?.Dispose();
                _bg = null;
            }
            base.Dispose(disposing);
        }
    }
}
