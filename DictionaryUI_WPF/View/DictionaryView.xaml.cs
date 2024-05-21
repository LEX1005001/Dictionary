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
        private Dictionary<string, List<Tuple<int, string, string>>> themeWords = new Dictionary<string, List<Tuple<int, string, string>>>();

        /// <summary>
        /// Иницальзация DictionaryView
        /// </summary>
        public DictionaryView()
        {
            InitializeComponent();
            LoadThemesAndWordsFromDatabase();
        }

        /// <summary>
        /// Загрузка тем и слов из БД
        /// </summary>
        private void LoadThemesAndWordsFromDatabase()
        {
            // Получение соединения с БД
            DataBaseHelper.Instance.ExecuteDbOperation(connection =>
            {
                // Загрузка тем из базы данных
                string queryThemes = "SELECT Name FROM Theme";
                // Создаем команду для выполнения запроса
                using (SQLiteCommand commandThemes = new SQLiteCommand(queryThemes, connection))
                {
                    // Выполняем запрос и получаем объект для чтения данных
                    using (SQLiteDataReader readerThemes = commandThemes.ExecuteReader())
                    {
                        // Перебираем все полученные строки
                        while (readerThemes.Read())
                        {
                            // Получаем название темы из текущей строки
                            string themeName = readerThemes["Name"].ToString();
                            // Добавляем тему в список тем
                            themesListBox.Items.Add(new ListBoxItem { Content = themeName });
                            // Добавляем новый словарь для темы
                            themeWords.Add(themeName, new List<Tuple<int, string, string>>());
                        }
                    }
                }
                // Загрузка слов и ID для каждой темы из базы данных
                foreach (string theme in themeWords.Keys)
                {
                    // Запрос для получения слов и их переводов
                    string queryWords = @"SELECT Word.Id, Word.thisWord, WordDictionary.Translation FROM WordDictionary 
                            JOIN Word ON WordDictionary.WordId = Word.Id 
                            JOIN Theme ON WordDictionary.ThemeId = Theme.Id
                            WHERE Theme.Name = @ThemeName";

                    // Создаем команду для выполнения запроса
                    using (SQLiteCommand commandWords = new SQLiteCommand(queryWords, connection))
                    {
                        // Добавляем параметр с названием темы
                        commandWords.Parameters.AddWithValue("@ThemeName", theme);
                        // Выполняем запрос и получаем объект для чтения данных
                        using (SQLiteDataReader readerWords = commandWords.ExecuteReader())
                        {
                            // Перебираем все полученные строки
                            while (readerWords.Read())
                            {
                                // Получаем идентификатор слова из текущей строки
                                int wordId = Convert.ToInt32(readerWords["Id"]);
                                // Получаем слово из текущей строки
                                string word = readerWords["thisWord"].ToString();
                                // Получаем перевод слова из текущей строки
                                string translation = readerWords["Translation"].ToString();
                                // Добавляем слово в список слов для текущей темы
                                themeWords[theme].Add(new Tuple<int, string, string>(wordId, word, translation));
                            }
                        }
                    }
                }
            });
        }

        /// <summary>
        /// Выбор темы и  выведение всех слов по этой теме
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThemesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
            string selectedTheme = (themesListBox.SelectedItem as ListBoxItem)?.Content.ToString();
            if (selectedTheme != null)
            {
                wordsListView.IsEnabled = true; // Возможно, потребуется включать ListView
                wordsListView.Items.Clear();
                foreach (var item in themeWords[selectedTheme])
                {
                    var lvi = new ListViewItem();
                    lvi.Content = new
                    {
                        ID = item.Item1,
                        Word = item.Item2,
                        Translation = item.Item3
                    };
                    wordsListView.Items.Add(lvi);
                }
            }
        }
    }
}
