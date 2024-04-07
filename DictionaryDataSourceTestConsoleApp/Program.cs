using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DictionatyDataSource;


namespace DictionaryDataSourceTestConsoleApp
{
    internal class Program
    {
       /* static SQLiteConnection connection;
        static SQLiteCommand command;*/
        public DataBase dataBase;
        static void Main(string[] args)
        {
            try
            {
                DataBase dataBase= new DataBase();
                dataBase.Connection.Open();
                Console.WriteLine("Connected!");
                dataBase.GetAll();
                Console.WriteLine();
                dataBase.GetAll_themes();
                Console.WriteLine();
                dataBase.GetAll_words();
                
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Ошибка доступа к базе данных. Исключение: {ex.Message}");
            }
            Console.Read();
        }
    }

}