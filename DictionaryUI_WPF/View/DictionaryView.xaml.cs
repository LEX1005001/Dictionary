﻿using System;
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
        private Dictionary<string, List<Tuple<int, string>>> themeWords = new Dictionary<string, List<Tuple<int, string>>>();

        public DictionaryView()
        {
            InitializeComponent();
            LoadThemesAndWordsFromDatabase();
        }

        private void LoadThemesAndWordsFromDatabase()
        {
            string connectionString = "Data Source=D:\\DataBase\\DictionaryDB.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Загрузка тем из базы данных
                string queryThemes = "SELECT Name FROM Theme";
                using (SQLiteCommand commandThemes = new SQLiteCommand(queryThemes, connection))
                {
                    using (SQLiteDataReader readerThemes = commandThemes.ExecuteReader())
                    {
                        while (readerThemes.Read())
                        {
                            string themeName = readerThemes["Name"].ToString();
                            themesListBox.Items.Add(new ListBoxItem { Content = themeName });
                            themeWords.Add(themeName, new List<Tuple<int, string>>());
                        }
                    }
                }

                // Загрузка слов и ID для каждой темы из базы данных
                foreach (string theme in themeWords.Keys)
                {
                    string queryWords = @"SELECT Word.Id, Word.thisWord FROM WordDictionary 
                                          JOIN Word ON WordDictionary.WordId = Word.Id 
                                          WHERE ThemeId = (SELECT Id FROM Theme WHERE Name = @ThemeName)";
                    using (SQLiteCommand commandWords = new SQLiteCommand(queryWords, connection))
                    {
                        commandWords.Parameters.AddWithValue("@ThemeName", theme);
                        using (SQLiteDataReader readerWords = commandWords.ExecuteReader())
                        {
                            while (readerWords.Read())
                            {
                                int wordId = Convert.ToInt32(readerWords["Id"]);
                                string word = readerWords["thisWord"].ToString();
                                themeWords[theme].Add(new Tuple<int, string>(wordId, word));
                            }
                        }
                    }
                }

                connection.Close();
            }
        }

        private void ThemesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedTheme = (themesListBox.SelectedItem as ListBoxItem)?.Content.ToString();

            if (selectedTheme != null)
            {
                wordsListView.Items.Clear();
                foreach (var item in themeWords[selectedTheme])
                {
                    wordsListView.Items.Add(new ListViewItem { Content = $"{item.Item1} - {item.Item2}" });
                }
            }
        }
    }
}
