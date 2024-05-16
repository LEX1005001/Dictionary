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
using System.Windows.Markup;

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
            using (var connection = DataBaseHelper.Instance.GetConnection())
            {
                LoadThemes(connection);
                LoadWordsForThemes(connection);
            }
        }

        private void LoadThemes(SQLiteConnection connection)
        {
            Themes.Clear();
            var command = new SQLiteCommand("SELECT Id, Name FROM Theme", connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Themes.Add(new Theme { Id = reader.GetInt32(0), Name = reader.GetString(1) });
                }
            }
        }

        private void LoadWordsForThemes(SQLiteConnection connection)
        {
            foreach (var theme in Themes)
            {
                Words.Clear(); // It should be considered if you really want to clear words for every theme.
                LoadWordsForTheme(theme.Id, connection);
            }
        }

        private void LoadWordsForTheme(int themeId, SQLiteConnection connection)
        {
            var command = new SQLiteCommand(@"SELECT Word.Id, Word.thisWord 
                                          FROM Word 
                                          JOIN WordDictionary ON Word.Id = WordDictionary.WordId 
                                          WHERE WordDictionary.ThemeId = @themeId", connection);
            command.Parameters.AddWithValue("@themeId", themeId);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Words.Add(reader.GetString(1));
                }
            }
        }
    }
}