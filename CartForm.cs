using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SportPitShop
{
    public class CartForm : NoFlickerForm
    {
        private readonly DataGridView grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            AllowUserToAddRows = false,
            ReadOnly = true,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.FromArgb(24, 26, 34),
            BorderStyle = BorderStyle.None
        };

        private readonly Label lblTotal = new Label { AutoSize = true, ForeColor = Color.White, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
        private readonly NeonButton btnClear = new NeonButton { Text = "Очистить", IsPrimary = false };
        private readonly NeonButton btnClose = new NeonButton { Text = "Закрыть", IsPrimary = false };

        public CartForm()
        {
            Text = "Корзина";
            Width = 780;
            Height = 520;
            Ui.ApplyBaseFormStyle(this);
            BackgroundImage = null;

            var bg = new NeonBackgroundPanel { Dock = DockStyle.Fill };
            Controls.Add(bg);

            var top = new Panel { Dock = DockStyle.Top, Height = 66, BackColor = Color.FromArgb(18, 18, 24) };
            bg.Controls.Add(grid);
            bg.Controls.Add(top);

            var lbl = new Label
            {
                Text = "Корзина",
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(16, 16)
            };

            lblTotal.Location = new Point(200, 20);

            btnClear.Location = new Point(520, 16);
            btnClear.Size = new Size(110, 34);
            btnClear.BorderColor = Ui.Accent2;

            btnClose.Location = new Point(640, 16);
            btnClose.Size = new Size(110, 34);
            btnClose.BorderColor = Ui.Accent2;

            btnClose.Clicked += (s, e) => Close();
            btnClear.Clicked += (s, e) => { Cart.Clear(); RefreshGrid(); };

            top.Controls.AddRange(new Control[] { lbl, lblTotal, btnClear, btnClose });

            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(18, 18, 24);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.EnableHeadersVisualStyles = false;
            grid.DefaultCellStyle.BackColor = Color.FromArgb(24, 26, 34);
            grid.DefaultCellStyle.ForeColor = Color.White;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(40, 50, 70);
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.RowHeadersVisible = false;

            Shown += (s, e) => RefreshGrid();
        }

        public void RefreshGrid()
        {
            var data = Cart.Items.Select(i => new
            {
                i.SKU,
                i.Name,
                Qty = i.Qty,
                Unit = i.FinalUnitPrice,
                Total = i.LineTotal
            }).ToList();

            grid.DataSource = data;
            lblTotal.Text = $"Итого: {Cart.TotalRUB} ₽  |  Товаров: {Cart.Count}";
        }
    }
}
