using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DictionaryUI_WPF.Model;
using DictionaryClassLibrary;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using DictionaryUI_WPF.Utilites;

namespace DictionaryUI_WPF.ViewModel
{
    public class DictionaryViewModel
    {
        public ObservableCollection<Theme> Themes { get; private set; } = new ObservableCollection<Theme>();
        public ObservableCollection<string> Words { get; private set; } = new ObservableCollection<string>();

        public DictionaryViewModel()
        {
            LoadThemesAndWordsFromDatabase();
        }

        private void LoadThemesAndWordsFromDatabase()
        {
            string connectionString = "Data Source=D:\\DataBase\\DictionaryDB.db;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                LoadThemes(connection);
                LoadWordsForThemes(connection);
                connection.Close();
            }
        }

        private void LoadThemes(SQLiteConnection connection)
        {
            string queryThemes = "SELECT Name FROM Theme";
            using (var commandThemes = new SQLiteCommand(queryThemes, connection))
            {
                using (var readerThemes = commandThemes.ExecuteReader())
                {
                    while (readerThemes.Read())
                    {
                        Themes.Add(new Theme { Name = readerThemes["Name"].ToString() });
                    }
                }
            }
        }

        private void LoadWordsForThemes(SQLiteConnection connection)
        {
            foreach (var theme in Themes)
            {
                string queryWords = @"SELECT Word.Id, Word.thisWord FROM WordDictionary 
                                  JOIN Word ON WordDictionary.WordId = Word.Id 
                                  WHERE ThemeId = (SELECT Id FROM Theme WHERE Name = @ThemeName)";

                using (var commandWords = new SQLiteCommand(queryWords, connection))
                {
                    commandWords.Parameters.AddWithValue("@ThemeName", theme.Name);

                    using (var readerWords = commandWords.ExecuteReader())
                    {
                        while (readerWords.Read())
                        {
                            var wordId = readerWords["Id"].ToString();
                            var word = readerWords["thisWord"].ToString();
                            Words.Add($"{wordId} - {word}");
                        }
                    }
                }
            }
        }
    }
}