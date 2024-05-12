﻿using System;
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

namespace DictionaryUI_WPF.ViewModel
{
    public class DeleteWordViewModel : INotifyPropertyChanged
    {
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
                LoadWordsForTheme(selectedTheme.Id);
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
            LoadThemes();

            DeleteThemeCommand = new RelayCommand(o => DeleteTheme(), o => selectedTheme != null);
            DeleteWordCommand = new RelayCommand(o => DeleteWord(), o => selectedWord != null);
        }

        private void LoadThemes()
        {
            Themes.Clear();
            using (var connection = new SQLiteConnection("Data Source=D:\\DataBase\\DictionaryDB.db;Version=3;"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"SELECT Id, Name FROM Theme";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Themes.Add(new Theme { Id = reader.GetInt32(0), Name = reader.GetString(1) });
                    }
                }
            }
        }

        private void LoadWordsForTheme(int themeId)
        {
            Words.Clear();
            using (var connection = new SQLiteConnection("Data Source=D:\\DataBase\\DictionaryDB.db;Version=3;"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"SELECT Word.Id, Word.thisWord 
                                        FROM Word 
                                        JOIN WordDictionary ON Word.Id = WordDictionary.WordId 
                                        WHERE WordDictionary.ThemeId = $themeId";
                command.Parameters.AddWithValue("$themeId", themeId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Words.Add(new Word { Id = reader.GetInt32(0), ThisWord = reader.GetString(1) });
                    }
                }
            }
        }

        private void DeleteTheme()
        {
            using (var connection = new SQLiteConnection("Data Source=D:\\DataBase\\DictionaryDB.db;Version=3;"))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    // Delete words from WordDictionary 
                    var command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM WordDictionary WHERE ThemeId = $themeId";
                    command.Parameters.AddWithValue("$themeId", selectedTheme.Id);
                    command.ExecuteNonQuery();

                    // Delete the theme
                    command.CommandText = "DELETE FROM Theme WHERE Id = $themeId";
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
            }
            LoadThemes();
            Words.Clear();
        }

        private void DeleteWord()
        {
            using (var connection = new SQLiteConnection("Data Source=D:\\DataBase\\DictionaryDB.db;Version=3;"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM WordDictionary WHERE WordId = $wordId";
                command.Parameters.AddWithValue("$wordId", selectedWord.Id);
                command.ExecuteNonQuery();
            }
            LoadWordsForTheme(selectedTheme.Id);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
