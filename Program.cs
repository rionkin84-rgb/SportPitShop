using System;
using System.Windows.Forms;

namespace SportPitShop
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try { Db.EnsureDatabaseAvailable(); }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось подготовить базу данных.\n\n" + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.Run(new Form1());
        }
    }
}
