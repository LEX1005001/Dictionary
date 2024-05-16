using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryUI_WPF.Utilites
{
    /// <summary>
    /// Класс для удобного подключения БД к проекту
    /// </summary>
    public class DataBaseHelper
    {
        private static DataBaseHelper instance;
        private static readonly object lockObject = new object();
        private SQLiteConnection connection;
        private readonly string connectionString = "Data Source=D:\\DataBase\\DictionaryDB.db;Version=3;";

        private DataBaseHelper()
        {
            connection = new SQLiteConnection(connectionString);
            connection.Open();
        }

        /// <summary>
        /// Выполнить операцию с Базой Данных
        /// </summary>
        /// <param name="operation"></param>
        public void ExecuteDbOperation(Action<SQLiteConnection> operation)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                operation(connection);
            }
        }

        public static DataBaseHelper Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new DataBaseHelper();
                    }
                    return instance;
                }
            }
        }

        public SQLiteConnection GetConnection()
        {
            var connection = new SQLiteConnection(connectionString);
            connection.Open();
            return connection;
        }

        public void CloseConnection()
        {
            connection.Close();
        }

    }
}

