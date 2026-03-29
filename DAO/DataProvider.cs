using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuanLyCuaHangMayTinh.DAO
{
    public class DataProvider
    {
        private static DataProvider instance;

        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return DataProvider.instance; }
            private set { DataProvider.instance = value; }
        }

        private const string DefaultConnectionString =
    "Data Source=localhost,1433;Initial Catalog=QLCHMT;User ID=sa;Password=25042004Ok;Encrypt=False;TrustServerCertificate=True";
        private const string ConnectionStringName = "QuanLyCuaHangMayTinh.Properties.Settings.QLCHMTConnectionString";

        private DataProvider() { }

        private string ConnectionString
        {
            get
            {
                string configuredConnectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName]?.ConnectionString;
                if (!string.IsNullOrWhiteSpace(configuredConnectionString))
                {
                    return configuredConnectionString;
                }

                return DefaultConnectionString;
            }
        }

        public DataTable executeQuery(string query, object[] paras = null)
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = CreateCommand(connection, query, paras))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    dataAdapter.Fill(data);
                }
            }

            return data;
        }

        public int executeNonQuery(string query, object[] paras = null)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = CreateCommand(connection, query, paras))
                {
                    return command.ExecuteNonQuery();
                }
            }
        }

        private SqlCommand CreateCommand(SqlConnection connection, string query, object[] paras)
        {
            SqlCommand command = new SqlCommand(query, connection);

            if (paras == null || paras.Length == 0)
            {
                return command;
            }

            List<string> parameterNames = Regex.Matches(query, @"@\w+")
                .Cast<Match>()
                .Select(match => match.Value)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (parameterNames.Count != paras.Length)
            {
                throw new ArgumentException("Số lượng tham số không khớp với câu truy vấn.");
            }

            for (int i = 0; i < parameterNames.Count; i++)
            {
                command.Parameters.AddWithValue(parameterNames[i], paras[i] ?? DBNull.Value);
            }

            return command;
        }
    }
}
