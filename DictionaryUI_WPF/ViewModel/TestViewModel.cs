using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DictionaryUI_WPF.Model;

namespace DictionaryUI_WPF.ViewModel
{
    public class TestViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<string> themes;
        public ObservableCollection<string> Themes
        {
            get => themes;
            set
            {
                themes = value;
                OnPropertyChanged(nameof(Themes));
            }
        }

        private string currentTheme;
        public string CurrentTheme
        {
            get => currentTheme;
            set
            {
                currentTheme = value;
                OnPropertyChanged(nameof(CurrentTheme));
                LoadWords(value);
            }
        }

        private string currentWord;
        public string CurrentWord
        {
            get => currentWord;
            private set
            {
                currentWord = value;
                OnPropertyChanged(nameof(CurrentWord));
            }
        }

        private string currentWordTranslation;
        private string resultMessage;
        public string ResultMessage
        {
            get => resultMessage;
            set
            {
                resultMessage = value;
                OnPropertyChanged(nameof(ResultMessage));
            }
        }

        private string translationInput;
        public string TranslationInput
        {
            get => translationInput;
            set
            {
                translationInput = value;
                OnPropertyChanged(nameof(TranslationInput));
            }
        }

        private int totalWordsTested;
        private int correctAnswers;

        private ObservableCollection<Tuple<string, string>> wordsPool; // Word and Translation
        private Random random = new Random();

        public ICommand CheckTranslationCommand { get; }

        public TestViewModel()
        {
            CheckTranslationCommand = new RelayCommand3(ValidateTranslation);
            LoadThemes();
        }

        private void LoadThemes()
        {
            Themes = new ObservableCollection<string>();
            using (var connection = new SQLiteConnection("Data Source=D:\\DataBase\\DictionaryDB.db;Version=3"))
            {
                connection.Open();
                string query = "SELECT Name FROM Theme";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Themes.Add(reader.GetString(0));
                }
            }
        }

        private void LoadWords(string theme)
        {
            wordsPool = new ObservableCollection<Tuple<string, string>>();
            using (var connection = new SQLiteConnection("Data Source=D:\\DataBase\\DictionaryDB.db;Version=3"))
            {
                connection.Open();
                string query = "SELECT Word.thisWord, WordDictionary.Translation FROM Word JOIN WordDictionary ON Word.Id = WordDictionary.WordId JOIN Theme ON Theme.Id = WordDictionary.ThemeId WHERE Theme.Name = @themeName";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@themeName", theme);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    wordsPool.Add(new Tuple<string, string>(reader.GetString(0), reader.GetString(1)));
                }
                NextWord();
                totalWordsTested = 0;
                correctAnswers = 0;
                ResultMessage = "";
            }
        }

        private void NextWord()
        {
            if (wordsPool.Count > 0)
            {
                int index = random.Next(wordsPool.Count);
                var wordPair = wordsPool[index];
                CurrentWord = wordPair.Item1;
                currentWordTranslation = wordPair.Item2;  // Сохраняем перевод для текущего слова
                wordsPool.RemoveAt(index);
            }
            else
            {
                CurrentWord = "";
                ResultMessage = $"Результат: {correctAnswers}/{totalWordsTested}";
            }
        }

        private void ValidateTranslation()
        {
            if (CurrentWord == null || currentWordTranslation == null) return;
            totalWordsTested++;
            if (translationInput.Equals(currentWordTranslation, StringComparison.OrdinalIgnoreCase))
            {
                correctAnswers++;
                ResultMessage = "Верно!";
            }
            else
            {
                ResultMessage = "Неверно! Правильный перевод: " + currentWordTranslation;
            }
            TranslationInput = "";
            NextWord();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand3 : ICommand
    {
        private readonly Action execute;

        public RelayCommand3(Action execute)
        {
            this.execute = execute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            execute();
        }
    }
}
