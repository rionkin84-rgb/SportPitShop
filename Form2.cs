using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SportPitShop
{
    public class Form2 : NoFlickerForm
    {
        private readonly Session _session;

        private readonly TextBox tbSearch = new TextBox();
        private readonly ComboBox cbCategory = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly Button btnAdvanced = new Button { Text = "Админ" };
        private readonly Button btnOrders = new Button { Text = "Заказы" };
        private readonly Button btnExit = new Button { Text = "Выход" };
        private readonly Button btnCart = new Button { Text = "🛒 Корзина" };
        private readonly Label lblCartBadge = new Label { AutoSize = true };

        private readonly DoubleBufferedFlowLayoutPanel flow = new DoubleBufferedFlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true,
            Padding = new Padding(18),
            BackColor = Color.Transparent
        };

        public Form2(Session session)
        {
            _session = session;

            Text = "Каталог";
            Width = 1200;
            Height = 760;
            Ui.ApplyBaseFormStyle(this);

            // No BackgroundImage here; we paint neon panel ourselves (no flicker)
            this.BackgroundImage = null;

            var bg = new NeonBackgroundPanel { Dock = DockStyle.Fill };
            Controls.Add(bg);

            var top = new Panel { Dock = DockStyle.Top, Height = 84, BackColor = Color.FromArgb(18, 18, 24) };
            bg.Controls.Add(flow);
            bg.Controls.Add(top);

            var lbl = new Label
            {
                Text = "Каталог SportPit",
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(18, 18)
            };

            tbSearch.SetBounds(320, 22, 320, 34);
            tbSearch.Font = new Font("Segoe UI", 11);
            tbSearch.BackColor = Color.FromArgb(28, 30, 40);
            tbSearch.ForeColor = Color.White;
            tbSearch.BorderStyle = BorderStyle.FixedSingle;

            cbCategory.SetBounds(650, 22, 220, 34);
            cbCategory.Font = new Font("Segoe UI", 11);
            cbCategory.BackColor = Color.FromArgb(28, 30, 40);
            cbCategory.ForeColor = Color.White;

            StyleButton(btnAdvanced, Ui.Accent2);
            StyleButton(btnOrders, Ui.Accent2);
            StyleButton(btnExit, Color.FromArgb(80, 80, 90));

            StyleButton(btnCart, Ui.Accent);
            btnCart.FlatAppearance.BorderColor = Ui.Accent;
            btnCart.ForeColor = Color.White;

            btnCart.SetBounds(885, 20, 100, 38);
            btnAdvanced.SetBounds(990, 20, 55, 38);
            btnOrders.SetBounds(1050, 20, 65, 38);
            btnExit.SetBounds(1120, 20, 60, 38);

lblCartBadge.BackColor = Color.FromArgb(240, 0, 0, 0);
lblCartBadge.ForeColor = Color.White;
lblCartBadge.Font = new Font("Segoe UI", 9, FontStyle.Bold);
lblCartBadge.Padding = new Padding(6, 2, 6, 2);
lblCartBadge.Location = new Point(btnCart.Right - 18, btnCart.Top - 6);
lblCartBadge.Text = "0";
lblCartBadge.Visible = true;


            btnExit.Click += (s, e) => Close();
            btnAdvanced.Click += (s, e) => { using (var f = new Form3(_session)) f.ShowDialog(); LoadProducts(); };
            btnOrders.Click += (s, e) => { using (var f = new Form4(_session)) f.ShowDialog(); };
            btnCart.Click += (s, e) => OpenCart();

            bool isStaff = _session.Role == "Администратор" || _session.Role == "Менеджер" || _session.Role == "Кладовщик" || _session.Role == "Оператор";
            btnOrders.Visible = isStaff;
            btnAdvanced.Visible = isStaff;

            tbSearch.TextChanged += (s, e) => LoadProducts();
            cbCategory.SelectedIndexChanged += (s, e) => LoadProducts();

            top.Controls.AddRange(new Control[] { lbl, tbSearch, cbCategory, btnCart, lblCartBadge, btnAdvanced, btnOrders, btnExit });

            Shown += (s, e) => { LoadCategories(); LoadProducts(); };
        }

        private void StyleButton(Button b, Color border)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderColor = border;
            b.FlatAppearance.BorderSize = 1;
            b.BackColor = Color.FromArgb(18, 18, 24);
            b.ForeColor = Color.White;
        }

        private void LoadCategories()
        {
            cbCategory.Items.Clear();
            cbCategory.Items.Add("Все категории");
            foreach (DataRow r in Db.Query("SELECT CategoryName FROM Categories ORDER BY CategoryName;").Rows)
                cbCategory.Items.Add(Convert.ToString(r["CategoryName"]) ?? "");
            cbCategory.SelectedIndex = 0;
        }

        private void LoadProducts()
        {
            flow.SuspendLayout();
            flow.Controls.Clear();

            string q = tbSearch.Text.Trim();
            object qParam = string.IsNullOrWhiteSpace(q) ? (object)DBNull.Value : "%" + q + "%";

            string where = "WHERE 1=1";
            if (!string.IsNullOrWhiteSpace(q)) where += " AND p.Name LIKE $q";
            if (cbCategory.SelectedIndex > 0) where += " AND c.CategoryName = $cat";

            object catParam = cbCategory.SelectedIndex > 0 ? cbCategory.SelectedItem.ToString() : (object)DBNull.Value;

            var dt = Db.Query(@"
SELECT 
  p.SKU, p.Name, p.Unit, p.PriceRUB, p.DiscountPct, p.StockQty, p.PhotoFile,
  c.CategoryName, m.ManufacturerName, s.SupplierName
FROM Products p
JOIN Categories c ON c.CategoryID = p.CategoryID
JOIN Manufacturers m ON m.ManufacturerID = p.ManufacturerID
JOIN Suppliers s ON s.SupplierID = p.SupplierID
" + where + @"
ORDER BY p.Name;",
                ("$q", qParam),
                ("$cat", catParam)
            );

            foreach (DataRow r in dt.Rows)
            {
                dynamic row = new
                {
                    SKU = Convert.ToString(r["SKU"]) ?? "",
                    Name = Convert.ToString(r["Name"]) ?? "",
                    Unit = Convert.ToString(r["Unit"]) ?? "",
                    PriceRUB = Convert.ToInt32(r["PriceRUB"]),
                    DiscountPct = Convert.ToInt32(r["DiscountPct"]),
                    StockQty = Convert.ToInt32(r["StockQty"]),
                    CategoryName = Convert.ToString(r["CategoryName"]) ?? "",
                    ManufacturerName = Convert.ToString(r["ManufacturerName"]) ?? "",
                    SupplierName = Convert.ToString(r["SupplierName"]) ?? "",
                    PhotoFile = Convert.ToString(r["PhotoFile"]) ?? ""
                };

                var card = new ProductCard();
                card.Bind(row);
                card.AddToCartClicked += (s, e) =>
                {
                    var data = card.GetCartData();
                    Cart.Add(data.sku, data.name, data.unitPrice, data.discountPct, 1);
                    UpdateCartBadge();
                };
                flow.Controls.Add(card);
            }

            flow.ResumeLayout();
        }
private void OpenCart()
{
    using (var f = new CartForm())
    {
        f.ShowDialog();
    }
    UpdateCartBadge();
}

private void UpdateCartBadge()
{
    lblCartBadge.Text = Cart.Count.ToString();
    lblCartBadge.Visible = true;
    lblCartBadge.Location = new Point(btnCart.Right - 18, btnCart.Top - 6);
}

    }
}