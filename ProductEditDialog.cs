using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SportPitShop
{
    public class ProductEditDialog : Form
    {
        private readonly TextBox tbSku = new TextBox();
        private readonly TextBox tbName = new TextBox();
        private readonly TextBox tbUnit = new TextBox();
        private readonly NumericUpDown numPrice = new NumericUpDown { Minimum = 1, Maximum = 1000000, Value = 1000 };
        private readonly NumericUpDown numDiscount = new NumericUpDown { Minimum = 0, Maximum = 90, Value = 0 };
        private readonly NumericUpDown numStock = new NumericUpDown { Minimum = 0, Maximum = 100000, Value = 0 };
        private readonly ComboBox cbCat = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly ComboBox cbMan = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly ComboBox cbSup = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly TextBox tbDesc = new TextBox { Multiline = true, ScrollBars = ScrollBars.Vertical };
        private readonly TextBox tbPhoto = new TextBox();
        private readonly Button btnBrowse = new Button { Text = "Выбрать фото" };

        private readonly Button btnOk = new Button { Text = "OK", DialogResult = DialogResult.OK };
        private readonly Button btnCancel = new Button { Text = "Отмена", DialogResult = DialogResult.Cancel };

        public ProductEditDialog(string sku = null)
        {
            Text = sku == null ? "Добавить товар" : "Редактировать товар";
            Width = 760;
            Height = 640;
            Ui.ApplyBaseFormStyle(this);

            var card = Ui.MakeCardPanel();
            card.Dock = DockStyle.Fill;
            Controls.Add(card);

            int y = 10;
            card.Controls.Add(MkLabel("Артикул", 12, y)); tbSku.SetBounds(160, y, 560, 26); y += 34;
            card.Controls.Add(MkLabel("Наименование", 12, y)); tbName.SetBounds(160, y, 560, 26); y += 34;
            card.Controls.Add(MkLabel("Ед. изм.", 12, y)); tbUnit.SetBounds(160, y, 160, 26);
            card.Controls.Add(MkLabel("Цена", 340, y)); numPrice.SetBounds(400, y, 120, 26);
            card.Controls.Add(MkLabel("Скидка %", 540, y)); numDiscount.SetBounds(620, y, 100, 26); y += 34;

            card.Controls.Add(MkLabel("Остаток", 12, y)); numStock.SetBounds(160, y, 120, 26); y += 34;

            card.Controls.Add(MkLabel("Категория", 12, y)); cbCat.SetBounds(160, y, 560, 26); y += 34;
            card.Controls.Add(MkLabel("Производитель", 12, y)); cbMan.SetBounds(160, y, 560, 26); y += 34;
            card.Controls.Add(MkLabel("Поставщик", 12, y)); cbSup.SetBounds(160, y, 560, 26); y += 34;

            card.Controls.Add(MkLabel("Фото (путь)", 12, y));
            tbPhoto.SetBounds(160, y, 430, 26);
            btnBrowse.SetBounds(600, y, 120, 26);
            btnBrowse.BackColor = Ui.Accent2;
            btnBrowse.FlatStyle = FlatStyle.Flat;
            btnBrowse.FlatAppearance.BorderSize = 0;
            btnBrowse.Click += (s, e) => BrowsePhoto();
            y += 34;

            card.Controls.Add(MkLabel("Описание", 12, y)); tbDesc.SetBounds(160, y, 560, 240); y += 250;

            btnOk.SetBounds(500, y, 105, 36);
            btnOk.BackColor = Ui.Accent;
            btnOk.ForeColor = Color.White;
            btnOk.FlatStyle = FlatStyle.Flat;
            btnOk.FlatAppearance.BorderSize = 0;

            btnCancel.SetBounds(615, y, 105, 36);

            card.Controls.AddRange(new Control[] {
                tbSku, tbName, tbUnit, numPrice, numDiscount, numStock,
                cbCat, cbMan, cbSup, tbPhoto, btnBrowse, tbDesc, btnOk, btnCancel
            });

            LoadLists();

            if (sku != null)
            {
                tbSku.ReadOnly = true;
                LoadExisting(sku);
            }

            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }

        private static Label MkLabel(string text, int x, int y)
        {
            return new Label { Text = text, AutoSize = true, Location = new Point(x, y + 5), ForeColor = Color.FromArgb(30, 30, 36) };
        }

        private void BrowsePhoto()
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Выберите фото товара";
                ofd.Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Все файлы|*.*";
                if (ofd.ShowDialog() != DialogResult.OK) return;

                string imagesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                Directory.CreateDirectory(imagesDir);

                string ext = Path.GetExtension(ofd.FileName);
                string safeName = "p_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ext;
                string dest = Path.Combine(imagesDir, safeName);

                File.Copy(ofd.FileName, dest, true);
                tbPhoto.Text = Path.Combine("Images", safeName);
            }
        }

        private void LoadLists()
        {
            cbCat.Items.Clear();
            cbMan.Items.Clear();
            cbSup.Items.Clear();

            foreach (DataRow r in Db.Query("SELECT CategoryName FROM Categories ORDER BY CategoryName;").Rows)
                cbCat.Items.Add(Convert.ToString(r["CategoryName"]) ?? "");
            foreach (DataRow r in Db.Query("SELECT ManufacturerName FROM Manufacturers ORDER BY ManufacturerName;").Rows)
                cbMan.Items.Add(Convert.ToString(r["ManufacturerName"]) ?? "");
            foreach (DataRow r in Db.Query("SELECT SupplierName FROM Suppliers ORDER BY SupplierName;").Rows)
                cbSup.Items.Add(Convert.ToString(r["SupplierName"]) ?? "");

            if (cbCat.Items.Count > 0) cbCat.SelectedIndex = 0;
            if (cbMan.Items.Count > 0) cbMan.SelectedIndex = 0;
            if (cbSup.Items.Count > 0) cbSup.SelectedIndex = 0;
        }

        private void LoadExisting(string sku)
        {
            var dt = Db.Query(@"
SELECT p.SKU, p.Name, p.Unit, p.PriceRUB, p.DiscountPct, p.StockQty, p.Description, p.PhotoFile,
       c.CategoryName, m.ManufacturerName, s.SupplierName
FROM Products p
JOIN Categories c ON c.CategoryID = p.CategoryID
JOIN Manufacturers m ON m.ManufacturerID = p.ManufacturerID
JOIN Suppliers s ON s.SupplierID = p.SupplierID
WHERE p.SKU = $sku
LIMIT 1;", ("$sku", sku));

            if (dt.Rows.Count == 0) return;
            var r = dt.Rows[0];

            tbSku.Text = Convert.ToString(r["SKU"]);
            tbName.Text = Convert.ToString(r["Name"]);
            tbUnit.Text = Convert.ToString(r["Unit"]);
            numPrice.Value = Convert.ToDecimal(r["PriceRUB"]);
            numDiscount.Value = Convert.ToDecimal(r["DiscountPct"]);
            numStock.Value = Convert.ToDecimal(r["StockQty"]);
            tbDesc.Text = Convert.ToString(r["Description"]);
            tbPhoto.Text = Convert.ToString(r["PhotoFile"]);

            cbCat.SelectedItem = Convert.ToString(r["CategoryName"]);
            cbMan.SelectedItem = Convert.ToString(r["ManufacturerName"]);
            cbSup.SelectedItem = Convert.ToString(r["SupplierName"]);
        }

        private void ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(tbSku.Text)) throw new Exception("Артикул обязателен.");
            if (string.IsNullOrWhiteSpace(tbName.Text)) throw new Exception("Наименование обязательно.");
            if (cbCat.SelectedItem == null) throw new Exception("Категория не выбрана.");
            if (cbMan.SelectedItem == null) throw new Exception("Производитель не выбран.");
            if (cbSup.SelectedItem == null) throw new Exception("Поставщик не выбран.");
        }

        public void Insert()
        {
            ValidateInputs();

            Db.Execute(@"
INSERT INTO Products (SKU, Name, Unit, PriceRUB, SupplierID, ManufacturerID, CategoryID, DiscountPct, StockQty, Description, PhotoFile)
VALUES (
  $sku, $name, $unit, $price,
  (SELECT SupplierID FROM Suppliers WHERE SupplierName = $sup),
  (SELECT ManufacturerID FROM Manufacturers WHERE ManufacturerName = $man),
  (SELECT CategoryID FROM Categories WHERE CategoryName = $cat),
  $disc, $stock, $desc, $photo
);",
            ("$sku", tbSku.Text.Trim()),
            ("$name", tbName.Text.Trim()),
            ("$unit", string.IsNullOrWhiteSpace(tbUnit.Text) ? "шт" : tbUnit.Text.Trim()),
            ("$price", (int)numPrice.Value),
            ("$sup", cbSup.SelectedItem.ToString()),
            ("$man", cbMan.SelectedItem.ToString()),
            ("$cat", cbCat.SelectedItem.ToString()),
            ("$disc", (int)numDiscount.Value),
            ("$stock", (int)numStock.Value),
            ("$desc", string.IsNullOrWhiteSpace(tbDesc.Text) ? (object)DBNull.Value : tbDesc.Text),
            ("$photo", string.IsNullOrWhiteSpace(tbPhoto.Text) ? (object)DBNull.Value : tbPhoto.Text)
            );
        }

        public void UpdateProduct()
        {
            ValidateInputs();

            Db.Execute(@"
UPDATE Products
SET Name=$name,
    Unit=$unit,
    PriceRUB=$price,
    SupplierID=(SELECT SupplierID FROM Suppliers WHERE SupplierName = $sup),
    ManufacturerID=(SELECT ManufacturerID FROM Manufacturers WHERE ManufacturerName = $man),
    CategoryID=(SELECT CategoryID FROM Categories WHERE CategoryName = $cat),
    DiscountPct=$disc,
    StockQty=$stock,
    Description=$desc,
    PhotoFile=$photo
WHERE SKU=$sku;",
            ("$sku", tbSku.Text.Trim()),
            ("$name", tbName.Text.Trim()),
            ("$unit", string.IsNullOrWhiteSpace(tbUnit.Text) ? "шт" : tbUnit.Text.Trim()),
            ("$price", (int)numPrice.Value),
            ("$sup", cbSup.SelectedItem.ToString()),
            ("$man", cbMan.SelectedItem.ToString()),
            ("$cat", cbCat.SelectedItem.ToString()),
            ("$disc", (int)numDiscount.Value),
            ("$stock", (int)numStock.Value),
            ("$desc", string.IsNullOrWhiteSpace(tbDesc.Text) ? (object)DBNull.Value : tbDesc.Text),
            ("$photo", string.IsNullOrWhiteSpace(tbPhoto.Text) ? (object)DBNull.Value : tbPhoto.Text)
            );
        }
    }
}
