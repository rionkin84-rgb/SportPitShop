using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace SportPitShop
{
    public class ProductCard : UserControl
    {
        private Image _img;
        private string _name = "";
        private string _meta = "";
        private string _price = "";
        private bool _hover;

        private string _sku = "";
        private int _unitPrice;
        private int _discountPct;

        private readonly NeonButton btnAdd = new NeonButton { Text = "В корзину", IsPrimary = true };

        public event EventHandler AddToCartClicked;

        public string SKU { get => _sku; }

        public ProductCard()
        {
            Width = 240;
            Height = 340;
            BackColor = Color.Transparent;
            Margin = new Padding(12);
            DoubleBuffered = true;

            btnAdd.Size = new Size(210, 34);
            btnAdd.Location = new Point(14, 296);
            btnAdd.GlowColor = Ui.Accent;
            btnAdd.BorderColor = Ui.Accent2;
            btnAdd.Clicked += (s, e) => AddToCartClicked?.Invoke(this, EventArgs.Empty);

            Controls.Add(btnAdd);

            Cursor = Cursors.Hand;
            MouseEnter += (s, e) => { _hover = true; Invalidate(); };
            MouseLeave += (s, e) => { _hover = false; Invalidate(); };
        }

        public (string sku, string name, int unitPrice, int discountPct) GetCartData()
            => (_sku, _name, _unitPrice, _discountPct);

        public void Bind(dynamic row)
        {
            _sku = row.SKU;
            _name = row.Name ?? "";
            _meta = string.Format("{0} • {1} • Остаток: {2}", row.CategoryName, row.ManufacturerName, row.StockQty);

            _discountPct = (int)row.DiscountPct;
            _unitPrice = (int)row.PriceRUB;

            int finalPrice = _discountPct > 0 ? (_unitPrice * (100 - _discountPct)) / 100 : _unitPrice;

            _price = _discountPct > 0
                ? string.Format("{0} ₽  (-{1}%)", finalPrice, _discountPct)
                : string.Format("{0} ₽", finalPrice);

            LoadPhoto(Convert.ToString(row.PhotoFile));
            Invalidate();
        }

        private void LoadPhoto(string photoFile)
        {
            _img?.Dispose();
            _img = null;

            try
            {
                if (string.IsNullOrWhiteSpace(photoFile)) return;

                string path = photoFile;
                if (!Path.IsPathRooted(path))
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);

                if (!File.Exists(path)) return;

                using (var tmp = new Bitmap(path))
                    _img = new Bitmap(tmp);
            }
            catch
            {
                _img = null;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            int radius = 18;

            using (var path = RoundedRect(rect, radius))
            {
                using (var bg = new SolidBrush(Ui.CardBg))
                    g.FillPath(bg, path);

                Color border = _hover ? Ui.Accent : Color.FromArgb(90, Ui.Accent2);
                float bw = _hover ? 2.6f : 1.4f;

                using (var pen = new Pen(border, bw))
                    g.DrawPath(pen, path);

                if (_hover)
                {
                    using (var penGlow = new Pen(Color.FromArgb(60, Ui.Accent), 8f))
                        g.DrawPath(penGlow, path);
                }
            }

            var imgRect = new Rectangle(14, 14, Width - 28, 150);
            using (var pathImg = RoundedRect(imgRect, 14))
            {
                g.SetClip(pathImg);
                if (_img != null)
                {
                    g.DrawImage(_img, imgRect);
                }
                else
                {
                    using (var br = new LinearGradientBrush(imgRect, Color.FromArgb(35, 38, 50), Color.FromArgb(20, 20, 26), 90f))
                        g.FillRectangle(br, imgRect);
                    DrawCentered(g, "NO PHOTO", imgRect, new Font("Segoe UI", 10, FontStyle.Bold), Color.FromArgb(140, 160, 180));
                }
                g.ResetClip();
                using (var p = new Pen(Color.FromArgb(70, Ui.Accent2), 1.2f))
                    g.DrawPath(p, pathImg);
            }

            var nameRect = new Rectangle(14, 178, Width - 28, 44);
            var metaRect = new Rectangle(14, 224, Width - 28, 40);
            var priceRect = new Rectangle(14, 262, Width - 28, 28);

            using (var fName = new Font("Segoe UI", 11, FontStyle.Bold))
            using (var fMeta = new Font("Segoe UI", 9, FontStyle.Regular))
            using (var fPrice = new Font("Segoe UI", 14, FontStyle.Bold))
            {
                TextRenderer.DrawText(g, _name, fName, nameRect, Color.White, TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis);
                TextRenderer.DrawText(g, _meta, fMeta, metaRect, Ui.Muted, TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis);
                TextRenderer.DrawText(g, _price, fPrice, priceRect, Ui.Accent, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
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

        private static void DrawCentered(Graphics g, string text, Rectangle r, Font f, Color c)
        {
            var sz = TextRenderer.MeasureText(text, f);
            var x = r.X + (r.Width - sz.Width) / 2;
            var y = r.Y + (r.Height - sz.Height) / 2;
            TextRenderer.DrawText(g, text, f, new Point(x, y), c);
        }
    }
}
