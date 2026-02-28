using System.Windows.Forms;

namespace SportPitShop
{
    public class NoFlickerPanel : Panel
    {
        public NoFlickerPanel()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }
    }
}
