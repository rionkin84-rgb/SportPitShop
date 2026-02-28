using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SportPitShop
{
    public class Form4 : NoFlickerForm
    {
        private readonly Session _session;

        private readonly DoubleBufferedFlowLayoutPanel flow = new DoubleBufferedFlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Padding = new Padding(14),
            BackColor = Color.White
        };

        private readonly ComboBox cbStatus = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly Button btnClose = new Button { Text = "Закрыть" };

        public Form4(Session session)
        {
            _session = session;

            Text = "Заказы";
            Width = 820;
            Height = 660;
            Ui.ApplyBaseFormStyle(this);
            

            var top = Ui.MakeTopBar("Заказы");

            cbStatus.SetBounds(180, 16, 220, 36);
            cbStatus.Items.AddRange(new object[] { "Все", "Завершен", "В обработке", "Отменен" });
            cbStatus.SelectedIndex = 0;
            cbStatus.SelectedIndexChanged += (s, e) => LoadOrders();

            btnClose.SetBounds(715, 16, 80, 36);
            btnClose.Click += (s, e) => Close();

            top.Controls.AddRange(new Control[] { cbStatus, btnClose });

            var hostPanel = new NoFlickerPanel { Dock = DockStyle.Fill, BackColor = Color.White };
var bg = new GradientBackgroundPanel { Dock = DockStyle.Fill };
hostPanel.Controls.Add(flow);
bg.Controls.Add(hostPanel);
Controls.Add(bg);
            Controls.Add(top);

            Shown += (s, e) => LoadOrders();
        }

        private void LoadOrders()
        {
            flow.Controls.Clear();

            string where = "";
            object stVal = DBNull.Value;

            if (cbStatus.SelectedIndex > 0)
            {
                where = "WHERE o.Status = $st";
                stVal = cbStatus.SelectedItem.ToString();
            }

            var dt = Db.Query(@"
SELECT
  o.OrderID,
  c.FullName AS Customer,
  o.OrderDate,
  o.DeliveryDate,
  pp.Address AS PickupAddress,
  o.Status,
  o.PickupCode,
  SUM(oi.Qty * oi.UnitPriceRUB * (100 - oi.DiscountPct) / 100.0) AS TotalRUB
FROM Orders o
JOIN Customers c ON c.CustomerID = o.CustomerID
JOIN PickupPoints pp ON pp.PickupPointID = o.PickupPointID
JOIN OrderItems oi ON oi.OrderID = o.OrderID
" + where + @"
GROUP BY o.OrderID, c.FullName, o.OrderDate, o.DeliveryDate, pp.Address, o.Status, o.PickupCode
ORDER BY o.OrderID DESC;", ("$st", stVal));

            foreach (DataRow r in dt.Rows)
            {
                dynamic row = new
                {
                    OrderID = Convert.ToInt32(r["OrderID"]),
                    Customer = Convert.ToString(r["Customer"]) ?? "",
                    OrderDate = Convert.ToString(r["OrderDate"]) ?? "",
                    PickupAddress = Convert.ToString(r["PickupAddress"]) ?? "",
                    Status = Convert.ToString(r["Status"]) ?? "",
                    PickupCode = Convert.ToInt32(r["PickupCode"]),
                    TotalRUB = Convert.ToDecimal(r["TotalRUB"])
                };

                var card = new OrderCard();
                card.Bind(row);

                card.Click += (s, e) => OpenOrder(card.OrderId);
                foreach (Control child in card.Controls)
                    child.Click += (s, e) => OpenOrder(card.OrderId);

                flow.Controls.Add(card);
            }
        }

        private void OpenOrder(int orderId)
        {
            using (var f = new Form5(_session, orderId))
                f.ShowDialog();
            LoadOrders();
        }
    }
}