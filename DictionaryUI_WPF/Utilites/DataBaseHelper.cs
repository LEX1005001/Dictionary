using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryUI_WPF.Utilites
{   ///Singlton-патерн проектирования

    /// <summary>
    /// Класс для удобного подключения БД к проекту
    /// </summary>
    public class DataBaseHelper
    {
        private static DataBaseHelper instance;
        private static readonly object lockObject = new object();
        private SQLiteConnection connection;

        /// <summary>
        /// *Ссылка на БД
        /// </summary>
        private readonly string connectionString = "Data Source=C:\\Users\\AleX6\\Desktop\\Test\\Dictionary\\DictionaryUI_WPF\\Utilites\\DictionaryDB.db;Version=3;";

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

        /// <summary>
        /// Объект для обеспечения потокобезопасности при создании экземпляра `DataBaseHelper`
        /// </summary>
        public static DataBaseHelper Instance
        {
            get
            {
                //избежание создания нескольких экземпляров в многопоточной среде
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

        /// <summary>
        /// Получение соединения с БД
        /// </summary>
        /// <returns></returns>
        public SQLiteConnection GetConnection()
        {
            var connection = new SQLiteConnection(connectionString);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Закрыть соединение с БД
        /// </summary>
        public void CloseConnection()
        {
            connection.Close();
        }

    }
}

