using DictionaryUI_WPF.Model;
using DictionaryUI_WPF.Utilites;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DictionaryUI_WPF.View
{

    public partial class DictionaryView : UserControl
    {
        private readonly ServerHttpClient _serverHttpClient = new ServerHttpClient();
        private Dictionary<string, List<Tuple<int, string, string>>> themeWords = new Dictionary<string, List<Tuple<int, string, string>>>();

        public DictionaryView()
        {
            InitializeComponent();
            Task.Run(() => LoadThemesAndWordsFromServerAsync().ConfigureAwait(false));
        }

        private async Task LoadThemesAndWordsFromServerAsync()
        {
            var themes = await _serverHttpClient.GetAllThemesAsync();
            await Dispatcher.InvokeAsync(() =>
            {
                foreach (var theme in themes)
                {
                    themesListBox.Items.Add(new ListBoxItem { Content = theme.Name });
                }
            });

            foreach (var theme in themes)
            {
                var words = await _serverHttpClient.GetWordsByThemeAsync(theme.Id);
                var wordList = new List<Tuple<int, string, string>>();
                foreach (var word in words)
                {
                    wordList.Add(new Tuple<int, string, string>(word.Id, word.Word, word.Translation));
                }
                themeWords.Add(theme.Name, wordList);
            }
        }

        private void ThemesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedTheme = (themesListBox.SelectedItem as ListBoxItem)?.Content.ToString();
            if (selectedTheme != null)
            {
                wordsListView.IsEnabled = true;
                wordsListView.Items.Clear();
                foreach (var item in themeWords[selectedTheme])
                {
                    var lvi = new ListViewItem
                    {
                        Content = new
                        {
                            ID = item.Item1,
                            Word = item.Item2,
                            Translation = item.Item3
                        }
                    };
                    wordsListView.Items.Add(lvi);
                }
            }
        }
    }
}