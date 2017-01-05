using LogLibrary;
using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataLayer
{
    public class DataSQL
    {
        private static string mysqlConnectionString = "Host=localhost;Port=3306;Database=serviceapp;Uid=root;Pwd=PRK@1987;default command timeout=3600;";

        public DataTable getData(string sqlQuery)
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlDataAdapter adp = new MySqlDataAdapter(sqlQuery, mysqlConnectionString))
                {
                    adp.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                FileLogger.AppendLog("Error", "DataLayer >> DataSQL >> getData >> ", ex.Message);
            }
            return dt;
        }
    }
}
