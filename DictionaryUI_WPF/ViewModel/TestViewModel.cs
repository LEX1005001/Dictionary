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
using DictionaryUI_WPF.Utilites;

namespace DictionaryUI_WPF.ViewModel
{
    public class TestViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ServerHttpClient serverHttpClient = new ServerHttpClient();
        private ObservableCollection<Theme> themes;
        public ObservableCollection<string> Themes
        {
            get => themes == null ? new ObservableCollection<string>() : new ObservableCollection<string>(themes.Select(t => t.Name).ToList());
            set
            {
                themes = new ObservableCollection<Theme>(value.Select(name => new Theme { Name = name }));
                OnPropertyChanged(nameof(Themes));
            }
        }

        private Theme currentTheme;
        public string CurrentTheme
        {
            get => currentTheme?.Name;
            set
            {
                currentTheme = themes.FirstOrDefault(t => t.Name == value);
                OnPropertyChanged(nameof(CurrentTheme));
                if (currentTheme != null)
                {
                    LoadWords(currentTheme.Id);
                }
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

        private ObservableCollection<Tuple<string, string[]>> wordsPool;
        private Random random = new Random();

        public ICommand CheckTranslationCommand { get; }

        public TestViewModel()
        {
            CheckTranslationCommand = new RelayCommand3(ValidateTranslation);
            LoadThemesAsync();
        }

        private async void LoadThemesAsync()
        {
            var themes = await serverHttpClient.GetAllThemesAsync();
            if (themes != null)
            {
                this.themes = new ObservableCollection<Theme>(themes);
                OnPropertyChanged(nameof(Themes));
            }
        }

        private async void LoadWords(int themeId)
        {
            var words = await serverHttpClient.GetWordsByThemeAsync(themeId);
            if (words != null)
            {
                wordsPool = new ObservableCollection<Tuple<string, string[]>>(
                    words.GroupBy(word => word.Word)
                         .Select(group => new Tuple<string, string[]>(group.Key, group.Select(w => w.Translation).ToArray())));

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
                currentWordTranslation = string.Join(", ", wordPair.Item2);  // Сохраняем переводы для текущего слова
                wordsPool.RemoveAt(index);
            }
            else
            {
                CurrentWord = "";
                ResultMessage = $"Результат: {correctAnswers}/{totalWordsTested}";
                UpdateResultImage(correctAnswers, totalWordsTested); // Метод для обновления изображения
            }
        }

        private void ValidateTranslation()
        {
            if (CurrentWord == null || currentWordTranslation == null) return;
            totalWordsTested++;
            var translationsArray = currentWordTranslation.Split(',').Select(t => t.Trim()).ToArray();
            if (translationsArray.Contains(translationInput, StringComparer.OrdinalIgnoreCase))
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

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string resultImagePath;
        public string ResultImagePath
        {
            get { return resultImagePath; }
            set
            {
                resultImagePath = value;
                OnPropertyChanged(nameof(ResultImagePath));
            }
        }

        private void UpdateResultImage(int correct, int total)
        {
            double score = (double)correct / total;

            if (score == 1)
            {
                ResultImagePath = "..\\Images\\score_Good.png"; // Полный путь к изображению для совершенного результата
            }
            else if (score >= 0.5)
            {
                ResultImagePath = "..\\Images\\score_Norm.png"; // Путь к изображению для половины или более
            }
            else
            {
                ResultImagePath = "..\\Images\\score_Sad.png"; // Путь к изображению для менее чем половины
            }


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

   
