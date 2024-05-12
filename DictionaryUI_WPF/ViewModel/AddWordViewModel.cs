using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using DictionaryUI_WPF.Model;
using DictionaryUI_WPF.Utilites;


namespace DictionaryUI_WPF.ViewModel
{
    public class AddWordViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Theme> Themes { get; set; } = new ObservableCollection<Theme>();
        private Theme selectedTheme;
        public Theme SelectedTheme
        {
            get => selectedTheme;
            set { selectedTheme = value; OnPropertyChanged("SelectedTheme"); }
        }

        public Theme NewTheme { get; set; } = new Theme();
        public Word NewWord { get; set; } = new Word();
        private string translation;
        public string Translation
        {
            get => translation;
            set { translation = value; OnPropertyChanged("Translation"); }
        }

        public ICommand AddWordToSelectedThemeCommand { get; set; }
        public ICommand CreateThemeAndAddWordCommand { get; set; }

        public AddWordViewModel()
        {
            LoadThemes();
            AddWordToSelectedThemeCommand = new RelayCommandDictionary(AddWordToSelectedTheme);
            CreateThemeAndAddWordCommand = new RelayCommandDictionary(CreateThemeAndAddWord);
        }

        private void LoadThemes()
        {
            var connection = new SQLiteConnection("Data Source=D:\\DataBase\\DictionaryDB.db;Version=3;");
            connection.Open();
            var command = new SQLiteCommand("SELECT * FROM Theme", connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Themes.Add(new Theme { Id = reader.GetInt32(0), Name = reader.GetString(1) });
            }
        }

        private void AddWordToSelectedTheme()
        {
            using (var connection = new SQLiteConnection("Data Source=D:\\DataBase\\DictionaryDB.db;Version=3;"))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    // Добавляем слово в таблицу Word
                    var wordCommand = new SQLiteCommand("INSERT INTO Word (thisWord) VALUES (@word); SELECT last_insert_rowid();", connection);
                    wordCommand.Parameters.AddWithValue("@word", NewWord.ThisWord);
                    var wordId = (long)wordCommand.ExecuteScalar();

                    // Добавляем слово и перевод в таблицу WordDictionary
                    var dictionaryCommand = new SQLiteCommand("INSERT INTO WordDictionary (ThemeId, WordId, Translation) VALUES (@themeId, @wordId, @translation)", connection);
                    dictionaryCommand.Parameters.AddWithValue("@themeId", SelectedTheme.Id);
                    dictionaryCommand.Parameters.AddWithValue("@wordId", wordId);
                    dictionaryCommand.Parameters.AddWithValue("@translation", Translation);
                    dictionaryCommand.ExecuteNonQuery();

                    transaction.Commit();
                }
            }
        }

        private void CreateThemeAndAddWord()
        {
            using (var connection = new SQLiteConnection("Data Source=D:\\DataBase\\DictionaryDB.db;Version=3;"))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    // Создаем новую тему
                    var themeCommand = new SQLiteCommand("INSERT INTO Theme (Name) VALUES (@name); SELECT last_insert_rowid();", connection);
                    themeCommand.Parameters.AddWithValue("@name", NewTheme.Name);
                    var themeId = (long)themeCommand.ExecuteScalar();

                    // Добавляем слово
                    var wordCommand = new SQLiteCommand("INSERT INTO Word (thisWord) VALUES (@word); SELECT last_insert_rowid();", connection);
                    wordCommand.Parameters.AddWithValue("@word", NewWord.ThisWord);
                    var wordId = (long)wordCommand.ExecuteScalar();

                    // Добавляем в словарь
                    var dictionaryCommand = new SQLiteCommand("INSERT INTO WordDictionary (ThemeId, WordId, Translation) VALUES (@themeId, @wordId, @translation)", connection);
                    dictionaryCommand.Parameters.AddWithValue("@themeId", themeId);
                    dictionaryCommand.Parameters.AddWithValue("@wordId", wordId);
                    dictionaryCommand.Parameters.AddWithValue("@translation", Translation);
                    dictionaryCommand.ExecuteNonQuery();

                    // Обновляем список тем в UI
                    Themes.Clear();
                    LoadThemes();

                    transaction.Commit();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}