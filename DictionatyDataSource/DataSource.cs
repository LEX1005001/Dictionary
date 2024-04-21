using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionatyDataSource
{
    public class DataBase
    {
        private SQLiteConnection connection = new SQLiteConnection("Data Source = D:\\DataBase\\DictionaryDB.db;Version=3; FailIfMissing=False");
        private SQLiteCommand command;
        

       
        public SQLiteCommand Command { get => command; set => command = value; }
        public SQLiteConnection Connection { get => connection; set => connection = value; }

        //Получение слов:

        /// <summary>
        /// Получить всё и вывести
        /// </summary>
        public void GetAll()
        {
            command = new SQLiteCommand(connection)
            {
                CommandText = "SELECT * FROM \"WordDictionary\";"
            };
            Console.WriteLine("Результат запроса GetAll:");
            DataTable data = new DataTable();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(data);
            Console.WriteLine($"Прочитано {data.Rows.Count} записей из таблицы БД");
            foreach (DataRow row in data.Rows)
            {
               Console.WriteLine($"id = {row.Field<Int64>("Id")} themeId = {row.Field<Int64>("ThemeId")} wordId = {row.Field<Int64>("WordId")} translation = {row.Field<string>("Translation")}");
            }
        }

        /// <summary>
        /// Получить все темы и вывести
        /// </summary>
        public void GetAll_themes()
        {
            command = new SQLiteCommand(connection)
            {
                CommandText = "SELECT * FROM \"Theme\";"
            };
            Console.WriteLine("Результат запроса GetAll_themes:");
            DataTable data = new DataTable();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(data);
            Console.WriteLine($"Прочитано {data.Rows.Count} записей из таблицы БД");
            foreach(DataRow row in data.Rows)
            {
                Console.WriteLine($"id = {row.Field<Int64>("Id")} name = {row.Field<string>("Name")}");
            }
        }
        /// <summary>
        /// Получение всех слов и вывод
        /// </summary>
        public void GetAll_words()
        {
            command = new SQLiteCommand(connection)
            {
                CommandText = "SELECT * FROM \"Word\";"
            };
            Console.WriteLine("Результат запроса GetAll_words:");
            DataTable data = new DataTable();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(data);
            Console.WriteLine($"Прочитано {data.Rows.Count} записей из таблицы БД");
            foreach (DataRow row in data.Rows)
            {
                Console.WriteLine($"id = {row.Field<Int64>("Id")} this_word = {row.Field<string>("thisWord")}");
            }
        }

        //Добавление:

        /// <summary>
        /// Добавление слова
        /// </summary>
        public void AddWord()
        {
            command = new SQLiteCommand(connection)
            {
                CommandText = ""
            };
        }

        /// <summary>
        /// Добавление темы
        /// </summary>
        public void AddTheme()
        {
            command = new SQLiteCommand(connection)
            {
                CommandText = ""
            };
        }

        //Удаление:

        /// <summary>
        /// Удаление слова по индексу
        /// </summary>
        public void RemoveWord()
        {
            command = new SQLiteCommand(connection)
            {
                CommandText=""
            };
        }

        /// <summary>
        /// Удаление темы по индексу
        /// </summary>
        public void RemoveTheme()
        {
            command = new SQLiteCommand(connection)
            {
                CommandText = ""
            };
        }
    }
    
}
