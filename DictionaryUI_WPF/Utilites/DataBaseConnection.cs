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
    public class DatabaseConnection
    {
        private static DatabaseConnection _instance;
        private static readonly object Lock = new object();
        private SQLiteConnection _connection;

        /// <summary>
        /// Подключение DataBase
        /// </summary>
        private DatabaseConnection()
        {
            string connectionString = "Data Source=D:\\DataBase\\DictionaryDB.db;Version=3;";
            _connection = new SQLiteConnection(connectionString);
            _connection.Open();
        }

        public static DatabaseConnection Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DatabaseConnection();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Соединение
        /// </summary>
        public SQLiteConnection Connection
        {
            get { return _connection; }
        }

        public void CloseConnection()
        {
            if (_connection != null)
            {
                _connection.Close();
            }
        }
    }
}
