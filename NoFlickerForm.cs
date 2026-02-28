using System.Windows.Forms;

namespace SportPitShop
{
    // Reduces WinForms flicker (especially with background images + scrolling)
    public class NoFlickerForm : Form
    {
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                // WS_EX_COMPOSITED: 0x02000000 (double-buffer the entire window)
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        public NoFlickerForm()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }
    }
}
