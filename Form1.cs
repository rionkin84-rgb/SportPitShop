using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SportPitShop
{
    public class Form1 : NoFlickerForm
    {
        private readonly TextBox tbLogin = new TextBox();
        private readonly TextBox tbPass  = new TextBox();
        private readonly Button  btnLogin = new Button { Text = "Войти" };
        private readonly Button  btnGuest = new Button { Text = "Гость" };

        public Form1()
        {
            Text = "Вход";
            Width = 560;
            Height = 460;
            Ui.ApplyBaseFormStyle(this);

            var card = new Panel { BackColor = Color.FromArgb(245, 255, 255, 255), Left = 40, Top = 30, Width = 470, Height = 370, BorderStyle = BorderStyle.FixedSingle };
            Controls.Add(card);

            var picLogo = new PictureBox
{
    Width = 440,
                Height = 96,
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(15, 16),
    Image = Ui.TryLoadImage(@"Images\logo.png")
};

            var lblTitle = new Label { Text = "", AutoSize = true, Font = new Font("Segoe UI", 20, FontStyle.Bold), Location = new Point(18, 18) };
            var lblSub = new Label { Text = "Вход в систему", AutoSize = true, ForeColor = Ui.Muted, Location = new Point(20, 118) };

            var lblLogin = new Label { Text = "Email", AutoSize = true, Location = new Point(20, 156) };
            tbLogin.SetBounds(20, 178, 390, 28);

            var lblPass = new Label { Text = "Пароль", AutoSize = true, Location = new Point(20, 216) };
            tbPass.SetBounds(20, 238, 390, 28);
            tbPass.UseSystemPasswordChar = true;

            btnLogin.SetBounds(20, 280, 280, 44);
            btnLogin.BackColor = Ui.Accent;
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;

            btnGuest.SetBounds(310, 280, 140, 44);

            btnLogin.Click += (s, e) => DoLogin();
            btnGuest.Click += (s, e) => OpenCatalog(new Session { Role = "Гость", DisplayName = "Гость" });

            var hint = new Label
            {
                Text = "Тест: admin@sportpit.ru / Adm1n!2026",
                AutoSize = true,
                ForeColor = Ui.Muted,
                Location = new Point(20, 346)
            };

            card.Controls.AddRange(new Control[] { picLogo,  lblTitle, lblSub, lblLogin, tbLogin, lblPass, tbPass, btnLogin, btnGuest, hint });
            AcceptButton = btnLogin;
        }

        private void DoLogin()
        {
            string login = tbLogin.Text.Trim();
            string pass = tbPass.Text;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Введите email и пароль.", "Проверка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dt = Db.Query(@"
SELECT e.EmployeeID, e.FullName, r.RoleName
FROM Employees e
JOIN Roles r ON r.RoleID = e.RoleID
WHERE LOWER(e.Email) = LOWER($login) AND e.PasswordPlain = $pass
LIMIT 1;",
                ("$login", login),
                ("$pass", pass)
            );

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Неверный email или пароль.", "Вход", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var row = dt.Rows[0];
            var session = new Session
            {
                EmployeeId = Convert.ToInt32(row["EmployeeID"]),
                DisplayName = Convert.ToString(row["FullName"]) ?? "Сотрудник",
                Role = Convert.ToString(row["RoleName"]) ?? "Сотрудник"
            };

            OpenCatalog(session);
        }

        private void OpenCatalog(Session session)
        {
            Hide();
            using (var f = new Form2(session))
                f.ShowDialog();
            Show();
        }
    }
}
