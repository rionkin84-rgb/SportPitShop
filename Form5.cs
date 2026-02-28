using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SportPitShop
{
    public class Form5 : NoFlickerForm
    {
        private readonly Session _session;
        private readonly int _orderId;

        private readonly Label lblHeader = new Label { AutoSize = true, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
        private readonly Label lblMeta = new Label { AutoSize = true, ForeColor = Ui.Muted };
        private readonly ComboBox cbStatus = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly Button btnSave = new Button { Text = "Сохранить" };
        private readonly Button btnClose = new Button { Text = "Закрыть" };

        private readonly DataGridView grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.White
        };

        public Form5(Session session, int orderId)
        {
            _session = session;
            _orderId = orderId;

            Text = "Заказ №" + orderId;
            Width = 980;
            Height = 680;
            Ui.ApplyBaseFormStyle(this);
            

            var top = new Panel { Dock = DockStyle.Top, Height = 98, BackColor = Color.FromArgb(235, 255, 255, 255) };

            lblHeader.Location = new Point(16, 14);
            lblMeta.Location = new Point(16, 44);

            cbStatus.SetBounds(680, 18, 160, 34);
            cbStatus.Items.AddRange(new object[] { "Завершен", "В обработке", "Отменен" });

            btnSave.SetBounds(850, 18, 110, 34);
            btnClose.SetBounds(850, 56, 110, 30);

            btnClose.Click += (s, e) => Close();
            btnSave.Click += (s, e) => SaveStatus();

            bool canEdit = _session.Role == "Администратор" || _session.Role == "Менеджер";
            cbStatus.Enabled = canEdit;
            btnSave.Enabled = canEdit;

            top.Controls.AddRange(new Control[] { lblHeader, lblMeta, cbStatus, btnSave, btnClose });

            var bg = new GradientBackgroundPanel { Dock = DockStyle.Fill };
var host = new NoFlickerPanel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(245, 255, 255, 255), Padding = new Padding(8) };
host.Controls.Add(grid);
bg.Controls.Add(host);
Controls.Add(bg);

            Controls.Add(top);

            Shown += (s, e) => LoadOrder();
        }

        private void LoadOrder()
        {
            var dt = Db.Query(@"
SELECT
  o.OrderID,
  c.FullName AS Customer,
  o.OrderDate,
  o.DeliveryDate,
  pp.Address AS PickupAddress,
  o.Status,
  o.PickupCode
FROM Orders o
JOIN Customers c ON c.CustomerID = o.CustomerID
JOIN PickupPoints pp ON pp.PickupPointID = o.PickupPointID
WHERE o.OrderID = $id
LIMIT 1;", ("$id", _orderId));

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Заказ не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            var r = dt.Rows[0];
            lblHeader.Text = "Заказ №" + _orderId + " — " + Convert.ToString(r["Status"]);
            lblMeta.Text = string.Format("Клиент: {0} | Дата: {1} | ПВЗ: {2} | Код: {3}",
                Convert.ToString(r["Customer"]), Convert.ToString(r["OrderDate"]),
                Convert.ToString(r["PickupAddress"]), Convert.ToString(r["PickupCode"]));

            cbStatus.SelectedItem = Convert.ToString(r["Status"]);

            var items = Db.Query(@"
SELECT 
  oi.OrderItemID AS '№',
  p.SKU AS 'Артикул',
  p.Name AS 'Товар',
  oi.Qty AS 'Кол-во',
  oi.UnitPriceRUB AS 'Цена',
  oi.DiscountPct AS 'Скидка %',
  ROUND(oi.Qty * oi.UnitPriceRUB * (100 - oi.DiscountPct) / 100.0, 2) AS 'Итого'
FROM OrderItems oi
JOIN Products p ON p.SKU = oi.SKU
WHERE oi.OrderID = $id
ORDER BY oi.OrderItemID;", ("$id", _orderId));

            grid.DataSource = items;
        }

        private void SaveStatus()
        {
            if (cbStatus.SelectedItem == null)
            {
                MessageBox.Show("Выберите статус.", "Проверка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var status = cbStatus.SelectedItem.ToString();
            Db.Execute("UPDATE Orders SET Status = $st WHERE OrderID = $id;", ("$st", status), ("$id", _orderId));
            LoadOrder();
            MessageBox.Show("Статус сохранён.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
