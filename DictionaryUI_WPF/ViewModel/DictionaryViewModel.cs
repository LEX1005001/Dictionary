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
using System.Windows.Threading;
using DictionaryUI_WPF.View;
using System.Runtime.CompilerServices;

namespace DictionaryUI_WPF.ViewModel
{
    public class DictionaryViewModel : INotifyPropertyChanged
    {
        private readonly ServerHttpClient _serverHttpClient = new ServerHttpClient();
        private Dictionary<string, List<Tuple<int, string, string>>> themeWords = new Dictionary<string, List<Tuple<int, string, string>>>();
        private ObservableCollection<string> _themes = new ObservableCollection<string>();
        private ObservableCollection<dynamic> _words = new ObservableCollection<dynamic>();

        public ObservableCollection<string> Themes
        {
            get => _themes;
            set
            {
                _themes = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<dynamic> Words
        {
            get => _words;
            set
            {
                _words = value;
                OnPropertyChanged();
            }
        }

        private string _selectedTheme;
        public string SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                if (_selectedTheme != value)
                {
                    _selectedTheme = value;
                    OnPropertyChanged();
                    LoadWordsForSelectedTheme();
                }
            }
        }

        public DictionaryViewModel()
        {
            Task.Run(() => LoadThemesAndWordsFromServerAsync().ConfigureAwait(false));
        }

        private async Task LoadThemesAndWordsFromServerAsync()
        {
            var themes = await _serverHttpClient.GetAllThemesAsync();
            foreach (var theme in themes)
            {
                Themes.Add(theme.Name);

                var words = await _serverHttpClient.GetWordsByThemeAsync(theme.Id);
                var wordList = new List<Tuple<int, string, string>>();
                foreach (var word in words)
                {
                    wordList.Add(new Tuple<int, string, string>(word.Id, word.Word, word.Translation));
                }
                themeWords.Add(theme.Name, wordList);
            }
        }

        private void LoadWordsForSelectedTheme()
        {
            if (SelectedTheme != null && themeWords.TryGetValue(SelectedTheme, out var wordList))
            {
                Words.Clear();
                foreach (var item in wordList)
                {
                    Words.Add(new
                    {
                        ID = item.Item1,
                        Word = item.Item2,
                        Translation = item.Item3
                    });
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}