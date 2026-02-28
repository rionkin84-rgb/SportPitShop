using System;
using System.Drawing;
using System.Windows.Forms;

namespace SportPitShop
{
    public class OrderCard : UserControl
    {
        private readonly Label _title = new Label { AutoSize = false, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
        private readonly Label _meta  = new Label { AutoSize = false, ForeColor = Ui.Muted };
        private readonly Label _sum   = new Label { AutoSize = false, Font = new Font("Segoe UI", 10, FontStyle.Bold) };

        public int OrderId { get; private set; }

        public OrderCard()
        {
            Height = 96;
            Width = 720;
            BackColor = Color.White;
            BorderStyle = BorderStyle.FixedSingle;

            _title.SetBounds(14, 14, 680, 22);
            _meta.SetBounds(14, 38, 680, 20);
            _sum.SetBounds(14, 62, 680, 22);

            Controls.AddRange(new Control[] { _title, _meta, _sum });
            Cursor = Cursors.Hand;
        }

        public void Bind(dynamic row)
        {
            OrderId = (int)row.OrderID;
            _title.Text = string.Format("Заказ №{0} — {1}", row.OrderID, row.Status);
            _meta.Text = string.Format("Клиент: {0} • Дата: {1} • ПВЗ: {2}", row.Customer, row.OrderDate, row.PickupAddress);
            _sum.Text = string.Format("Сумма: {0:0.00} ₽ • Код: {1}", Convert.ToDecimal(row.TotalRUB), row.PickupCode);
        }
    }
}
