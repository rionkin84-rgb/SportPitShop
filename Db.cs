using System;
using System.Data;
using System.IO;
using System.Data.SQLite;

namespace SportPitShop
{
    public static class Db
    {
        private const string DbFileName = "SportPitShop.sqlite";
        public static string DbPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DbFileName);

        public static void EnsureDatabaseAvailable()
        {
            if (!File.Exists(DbPath))
                throw new FileNotFoundException("Файл базы данных не найден рядом с приложением: " + DbPath);
        }

        private static SQLiteConnection CreateConnection()
        {
            var cs = new SQLiteConnectionStringBuilder { DataSource = DbPath, ForeignKeys = true }.ToString();
            return new SQLiteConnection(cs);
        }

        public static DataTable Query(string sql, params (string name, object value)[] parameters)
        {
            using (var con = CreateConnection())
            {
                con.Open();
                using (var cmd = new SQLiteCommand(sql, con))
                {
                    foreach (var p in parameters)
                        cmd.Parameters.AddWithValue(p.name, p.value ?? DBNull.Value);

                    using (var da = new SQLiteDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public static int Execute(string sql, params (string name, object value)[] parameters)
        {
            using (var con = CreateConnection())
            {
                con.Open();
                using (var cmd = new SQLiteCommand(sql, con))
                {
                    foreach (var p in parameters)
                        cmd.Parameters.AddWithValue(p.name, p.value ?? DBNull.Value);
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
