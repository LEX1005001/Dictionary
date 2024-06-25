using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DictionaryUI_WPF.Model;
using DictionaryClassLibrary;
using DictionaryUI_WPF.Utilites;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Windows.Input;
using System.Windows.Forms;

namespace DictionaryUI_WPF.ViewModel
{
    public class DeleteWordViewModel : INotifyPropertyChanged
    {
        private ServerHttpClient _serverHttpClient = new ServerHttpClient();

        private ObservableCollection<Theme> themes;
        private ObservableCollection<Word> words;
        private Theme selectedTheme;
        private Word selectedWord;

        public ObservableCollection<Theme> Themes
        {
            get => themes;
            set
            {
                themes = value;
                OnPropertyChanged(nameof(Themes));
            }
        }

        public ObservableCollection<Word> Words
        {
            get => words;
            set
            {
                words = value;
                OnPropertyChanged(nameof(Words));
            }
        }

        public Theme SelectedTheme
        {
            get => selectedTheme;
            set
            {
                selectedTheme = value;
                OnPropertyChanged(nameof(SelectedTheme));
                if (selectedTheme != null)
                {
                    LoadWordsForTheme(selectedTheme.Id);
                }
                else
                {
                    Words.Clear();
                }
            }
        }

        public Word SelectedWord
        {
            get => selectedWord;
            set
            {
                selectedWord = value;
                OnPropertyChanged(nameof(SelectedWord));
            }
        }

        public ICommand DeleteThemeCommand { get; }
        public ICommand DeleteWordCommand { get; }

        public DeleteWordViewModel()
        {
            Themes = new ObservableCollection<Theme>();
            Words = new ObservableCollection<Word>();
            LoadThemesAsync();

            DeleteThemeCommand = new RelayCommand(o => DeleteThemeAsync(), o => selectedTheme != null);
            DeleteWordCommand = new RelayCommand(o => DeleteWordAsync(), o => selectedWord != null);
        }

        private async void LoadThemesAsync()
        {
            try
            {
                var themes = await _serverHttpClient.GetAllThemesAsync();
                foreach (var theme in themes)
                {
                    Themes.Add(theme); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void LoadWordsForTheme(int themeId)
        {
            Words.Clear();
            try
            {
                var wordsByTheme = await _serverHttpClient.GetWordsByThemeAsync(themeId);
                foreach (var word in wordsByTheme)
                {
                    Words.Add(new Word { Id = word.Id, ThisWord = word.Word });
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message);
            }
        }

        private async Task DeleteThemeAsync()
        {
            await _serverHttpClient.DeleteThemeWithWordsAsync(selectedTheme.Id);

            Themes.Remove(selectedTheme);
            Words.Clear();
            selectedTheme = null;
            OnPropertyChanged(nameof(SelectedTheme));

           
        }

        private async Task DeleteWordAsync()
        {
            try
            {
                await _serverHttpClient.DeleteWord_TrAsync(selectedWord.Id);
                LoadWordsForTheme(selectedTheme.Id);
            }
            catch (Exception ex)
            {
                // Обработка ошибки
                // Например, вывести сообщение пользователю
                MessageBox.Show(ex.Message);
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
