using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SportPitShop
{
    public static class Ui
    {
        public static readonly Color Accent = Color.FromArgb(0, 255, 209);
        public static readonly Color Accent2 = Color.FromArgb(74, 126, 255);
        public static readonly Color CardBg = Color.FromArgb(24, 26, 34);
        public static readonly Color Muted = Color.FromArgb(150, 160, 180);

        public static Image BackgroundImage
        {
            get
            {
                try
                {
                    var p = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "background.jpg");
                    if (File.Exists(p)) return Image.FromFile(p);
                }
                catch { }
                return null;
            }
        }

        public static Image TryLoadImage(string relativePath)
{
    try
    {
        var p = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
        if (File.Exists(p)) return Image.FromFile(p);
    }
    catch { }
    return null;
}

public static Image PlaceholderImage

        {
            get
            {
                try
                {
                    var p = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "placeholder.png");
                    if (File.Exists(p)) return Image.FromFile(p);
                }
                catch { }
                return null;
            }
        }

        public static void ApplyBaseFormStyle(Form f)
        {
            f.StartPosition = FormStartPosition.CenterScreen;
            f.Font = new Font("Segoe UI", 10);
            f.FormBorderStyle = FormBorderStyle.FixedDialog;
            f.MaximizeBox = false;
            f.BackgroundImage = BackgroundImage;
            f.BackgroundImageLayout = ImageLayout.Stretch;
            f.DoubleBuffered(true);
        }

        public static Panel MakeTopBar(string title)
        {
            var top = new NoFlickerPanel
            {
                Dock = DockStyle.Top,
                Height = 68,
                BackColor = Color.White
            };

            var lbl = new Label
            {
                Text = title,
                AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(16, 22),
                ForeColor = Color.FromArgb(20, 20, 26)
            };

            top.Controls.Add(lbl);
            return top;
        }

        public static Panel MakeCardPanel()
        {
            return new Panel
            {
                BackColor = Color.FromArgb(240, 255, 255, 255),
                Padding = new Padding(14),
                Margin = new Padding(14),
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        public static void DoubleBuffered(this Control control, bool enable)
        {
            var prop = typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            prop?.SetValue(control, enable, null);
        }
    }
}
