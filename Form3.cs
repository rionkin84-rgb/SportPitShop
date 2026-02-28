using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SportPitShop
{
    public class Form3 : NoFlickerForm
    {
        private readonly Session _session;

        private readonly TextBox tbSearch = new TextBox();
        private readonly ComboBox cbCategory = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly ComboBox cbManufacturer = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly ComboBox cbSupplier = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly ComboBox cbSort = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };

        private readonly Button btnAdd = new Button { Text = "Добавить" };
        private readonly Button btnEdit = new Button { Text = "Изменить" };
        private readonly Button btnDelete = new Button { Text = "Удалить" };
        private readonly Button btnClose = new Button { Text = "Закрыть" };

        private readonly DataGridView grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.White
        };

        public Form3(Session session)
        {
            _session = session;

            Text = "Каталог (админ)";
            Width = 1060;
            Height = 700;
            Ui.ApplyBaseFormStyle(this);
            

            var top = new Panel { Dock = DockStyle.Top, Height = 112, BackColor = Color.FromArgb(235, 255, 255, 255) };

            var lblSearch = new Label { Text = "Поиск:", AutoSize = true, Location = new Point(12, 18) };
            tbSearch.SetBounds(70, 14, 260, 28);

            cbCategory.SetBounds(342, 14, 200, 28);
            cbManufacturer.SetBounds(550, 14, 200, 28);
            cbSupplier.SetBounds(758, 14, 280, 28);

            cbSort.SetBounds(12, 60, 210, 28);
            cbSort.Items.AddRange(new object[] { "По названию", "Цена ↑", "Цена ↓", "Остаток ↓" });
            cbSort.SelectedIndex = 0;

            btnAdd.SetBounds(240, 58, 110, 34);
            btnEdit.SetBounds(360, 58, 110, 34);
            btnDelete.SetBounds(480, 58, 110, 34);
            btnClose.SetBounds(928, 58, 110, 34);

            btnClose.Click += (s, e) => Close();

            tbSearch.TextChanged += (s, e) => LoadGrid();
            cbCategory.SelectedIndexChanged += (s, e) => LoadGrid();
            cbManufacturer.SelectedIndexChanged += (s, e) => LoadGrid();
            cbSupplier.SelectedIndexChanged += (s, e) => LoadGrid();
            cbSort.SelectedIndexChanged += (s, e) => LoadGrid();

            btnAdd.Click += (s, e) => AddProduct();
            btnEdit.Click += (s, e) => EditSelected();
            btnDelete.Click += (s, e) => DeleteSelected();

            bool isAdmin = _session.Role == "Администратор";
            btnAdd.Enabled = isAdmin;
            btnEdit.Enabled = isAdmin;
            btnDelete.Enabled = isAdmin;

            top.Controls.AddRange(new Control[] { lblSearch, tbSearch, cbCategory, cbManufacturer, cbSupplier, cbSort, btnAdd, btnEdit, btnDelete, btnClose });

            var bg = new GradientBackgroundPanel { Dock = DockStyle.Fill };
var host = new NoFlickerPanel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(245, 255, 255, 255), Padding = new Padding(8) };
host.Controls.Add(grid);
bg.Controls.Add(host);
Controls.Add(bg);

            Controls.Add(top);

            Shown += (s, e) => { LoadFilters(); LoadGrid(); };
        }

        private void LoadFilters()
        {
            cbCategory.Items.Clear();
            cbManufacturer.Items.Clear();
            cbSupplier.Items.Clear();

            cbCategory.Items.Add("Все категории");
            cbManufacturer.Items.Add("Все производители");
            cbSupplier.Items.Add("Все поставщики");

            foreach (DataRow r in Db.Query("SELECT CategoryName FROM Categories ORDER BY CategoryName;").Rows)
                cbCategory.Items.Add(Convert.ToString(r["CategoryName"]) ?? "");
            foreach (DataRow r in Db.Query("SELECT ManufacturerName FROM Manufacturers ORDER BY ManufacturerName;").Rows)
                cbManufacturer.Items.Add(Convert.ToString(r["ManufacturerName"]) ?? "");
            foreach (DataRow r in Db.Query("SELECT SupplierName FROM Suppliers ORDER BY SupplierName;").Rows)
                cbSupplier.Items.Add(Convert.ToString(r["SupplierName"]) ?? "");

            cbCategory.SelectedIndex = 0;
            cbManufacturer.SelectedIndex = 0;
            cbSupplier.SelectedIndex = 0;
        }

        private void LoadGrid()
        {
            string where = "WHERE 1=1";
            string search = tbSearch.Text.Trim();

            if (!string.IsNullOrWhiteSpace(search))
                where += " AND p.Name LIKE $q";
            if (cbCategory.SelectedIndex > 0)
                where += " AND c.CategoryName = $cat";
            if (cbManufacturer.SelectedIndex > 0)
                where += " AND m.ManufacturerName = $man";
            if (cbSupplier.SelectedIndex > 0)
                where += " AND s.SupplierName = $sup";

            string orderBy = "ORDER BY p.Name ASC";
            if (cbSort.SelectedIndex == 1) orderBy = "ORDER BY p.PriceRUB ASC";
            if (cbSort.SelectedIndex == 2) orderBy = "ORDER BY p.PriceRUB DESC";
            if (cbSort.SelectedIndex == 3) orderBy = "ORDER BY p.StockQty DESC";

            string sql = @"
SELECT 
  p.SKU,
  p.Name AS 'Наименование',
  c.CategoryName AS 'Категория',
  m.ManufacturerName AS 'Производитель',
  s.SupplierName AS 'Поставщик',
  p.Unit AS 'Ед.',
  p.PriceRUB AS 'Цена',
  p.DiscountPct AS 'Скидка %',
  p.StockQty AS 'Остаток',
  p.PhotoFile AS 'Фото'
FROM Products p
JOIN Categories c ON c.CategoryID = p.CategoryID
JOIN Manufacturers m ON m.ManufacturerID = p.ManufacturerID
JOIN Suppliers s ON s.SupplierID = p.SupplierID
" + where + @"
" + orderBy + ";";

            object cat = cbCategory.SelectedIndex > 0 ? (object)cbCategory.SelectedItem.ToString() : DBNull.Value;
            object man = cbManufacturer.SelectedIndex > 0 ? (object)cbManufacturer.SelectedItem.ToString() : DBNull.Value;
            object sup = cbSupplier.SelectedIndex > 0 ? (object)cbSupplier.SelectedItem.ToString() : DBNull.Value;

            var dt = Db.Query(sql, ("$q", "%" + search + "%"), ("$cat", cat), ("$man", man), ("$sup", sup));
            grid.DataSource = dt;
        }

        private string SelectedSku()
        {
            if (grid.CurrentRow != null && grid.CurrentRow.Cells.Count > 0)
                return Convert.ToString(grid.CurrentRow.Cells["SKU"].Value);
            return null;
        }

        private void AddProduct()
        {
            using (var dlg = new ProductEditDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    dlg.Insert();
                    LoadFilters();
                    LoadGrid();
                }
            }
        }

        private void EditSelected()
        {
            var sku = SelectedSku();
            if (string.IsNullOrWhiteSpace(sku)) return;

            using (var dlg = new ProductEditDialog(sku))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    dlg.UpdateProduct();
                    LoadGrid();
                }
            }
        }

        private void DeleteSelected()
        {
            var sku = SelectedSku();
            if (string.IsNullOrWhiteSpace(sku)) return;

            if (MessageBox.Show("Удалить товар " + sku + "?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            Db.Execute("DELETE FROM Products WHERE SKU = $sku;", ("$sku", sku));
            LoadGrid();
        }
    }
}
