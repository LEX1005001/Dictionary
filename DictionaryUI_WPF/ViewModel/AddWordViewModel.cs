using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using DictionaryUI_WPF.Model;
using DictionaryUI_WPF.Utilites;
using System.Net.Http;



namespace DictionaryUI_WPF.ViewModel
{
    public class AddWordViewModel : INotifyPropertyChanged
    {
        private ServerHttpClient _serverHttpClient = new ServerHttpClient();

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

        private async void LoadThemes()
        {
            Themes.Clear();
            var themesFromServer = await _serverHttpClient.GetAllThemesAsync();
            foreach (var theme in themesFromServer)
            {
                Themes.Add(theme);
            }
        }

        private async void AddWordToSelectedTheme()
        {
            var wordToAdd = new Word_Tr
            {
                Word = NewWord.ThisWord,
                Translation = Translation
            };

            try
            {
                var wordsForTheme = await _serverHttpClient.AddWordToThemeAsync(SelectedTheme.Id, wordToAdd);
                // Вывод сообщения о успешном добавлении слова.
                MessageBox.Show("Слово успешно добавлено к теме.");

            }
            catch (HttpRequestException e)
            {
                MessageBox.Show($"Ошибка при добавлении слова к теме: {e.Message}");
            }
        }

        private async void CreateThemeAndAddWord()
        {
            try
            {
                bool result = await _serverHttpClient.AddNewThemeWithWordAsync(NewTheme.Name, NewWord.ThisWord, Translation);
                if (result)
                {
                    MessageBox.Show("Тема и слово успешно добавлены");
                    var themesFromServer = await _serverHttpClient.GetAllThemesAsync(); // Обновляем список тем асинхронно после добавления новой темы
                    foreach (var theme in themesFromServer)
                    {
                        Themes.Add(theme);
                    } 
                }
                else
                {
                    MessageBox.Show("Ошибка при добавлении темы и слова");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}