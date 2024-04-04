using DictionaryClassLibrary;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

public class DictionaryWords
{
    private List<Word_Translation> words;
    private string theme;

    /// <summary>
    /// Тема
    /// </summary>
    public string Theme
    {
        get => theme;
        set => theme = value;
    }

    /// <summary>
    /// Список Word_Translation(слов и переводов)
    /// </summary>
    public List<Word_Translation> Words
    {
        get => words;
        set => words = value;
    }

    /// <summary>
    /// Полный конструктор словаря
    /// </summary>
    /// <param name="words_">список слов с переводом</param>
    /// <param name="theme_"> тема словаря</param>
    public DictionaryWords(List<Word_Translation> words_, string theme_)
    {
        this.words = words_;
        this.theme = theme_;
    }

    /// <summary>
    /// Конструктор без данных
    /// </summary>
    public DictionaryWords()
    {
        this.theme = string.Empty;
        this.words = new List<Word_Translation>();
    }

    /// <summary>
    /// Проверка на пустой словарь
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty()
    {
        if (this.words.Count == 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Добавление слова в словарь по Word_Translation
    /// </summary>
    /// <param name="word_Translation"></param>
    public void AddWord(Word_Translation word_Translation)
    {
        this.words.Add(word_Translation);
    }

    /// <summary>
    /// Добавление слова в словарь по слову и переводу
    /// </summary>
    /// <param name="word"></param>
    /// <param name="translation"></param>
    public void AddWord(string word, string translation)
    {
        Word_Translation newWord = new Word_Translation(word, translation);
        this.words.Add(newWord);
    }

    /// <summary>
    /// Выводит кол-во слов в словаре
    /// </summary>
    /// <returns>размер словаря</returns>
    public int GetSize()
    {
        return this.words.Count;
    }
    /// <summary>
    /// Вывод темы словаря и всех слов в нём
    /// </summary>
    public void Info()
    {
        if (words == null || words.Count == 0)
        {
            Console.WriteLine("Словарь пуст.");
        }
        else
        {
            Console.WriteLine($"Тема: {theme}");
            Console.WriteLine("Список слов в словаре:");
            foreach (Word_Translation word in words)
            {
                Console.WriteLine($"Слово: {word.Word}, Перевод: {word.Translation}");
            }
        }
    }

    /// <summary>
    /// Получение всех тем из библиотеки словарей
    /// </summary>
    /// <param name="libraries">словарь</param>
    /// <returns>темы словарей</returns>
    public static List<DictionaryWords> GetAllThemesFromLibrary(List<DictionaryWords> libraries)
    {
        return libraries;
    }

    /// <summary>
    /// Вывод слов по теме словаря
    /// </summary>
    /// <param name="theme">тема</param>
    /// <returns>слова,по данной теме</returns>
    public List<Word_Translation> GetWordsByTheme(string theme)
    {
        List<Word_Translation> wordsWithTheme = new List<Word_Translation>();

        if (Theme == theme)
        {
            wordsWithTheme.AddRange(Words);
        }

        return wordsWithTheme;
    }

    /// <summary>
    /// Добавление словаря в библиотеку с проверкой на оригинальность
    /// </summary>
    /// <param name="libraries">библиотека</param>
    /// <param name="newDictionary">словарь,который мы хотим добавить</param>
    public static void AddDictionaryToList(List<DictionaryWords> libraries, DictionaryWords newDictionary)
    {
        var existingDictionary = libraries.FirstOrDefault(d => d.Theme == newDictionary.Theme);

        if (existingDictionary != null)
        {
            foreach (var word in newDictionary.Words)
            {
                if (existingDictionary.Words.All(w => w.Word != word.Word))
                {
                    existingDictionary.Words.Add(word);
                }
            }
        }
        else
        {
            libraries.Add(newDictionary);
        }
    }

    /// <summary>
    /// Сохранение словарей в формате xml
    /// </summary>
    /// <param name="filePath"></param>
    public void SaveDictionaryToXml(string filePath)
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DictionaryWords));
            serializer.Serialize(fileStream, this);
        }
    }

    /// <summary>
    /// Загрузка словарей из файла xml
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static DictionaryWords LoadDictionaryFromXml(string filePath)
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DictionaryWords));
            return (DictionaryWords)serializer.Deserialize(fileStream);
        }
    }

    /// <summary>
    /// Сохранение библиотеки словарей в формате xml
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="libraries"></param>
    public static void SaveLibrariesToXml(string filePath, List<DictionaryWords> libraries)
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<DictionaryWords>));
            serializer.Serialize(fileStream, libraries);
        }
    }

    /// <summary>
    /// Загрузка библиотек словарей из файла xml
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static List<DictionaryWords> LoadLibrariesFromXml(string filePath)
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<DictionaryWords>));
            return (List<DictionaryWords>)serializer.Deserialize(fileStream);
        }
    }

    /// <summary>
    /// Удаляет указанное слово из словаря
    /// </summary>
    /// <param name="wordToRemove">удаляемое слово</param>
    public void RemoveWord(string wordToRemove)
    {
        var wordToRemoveLower = wordToRemove.ToLower(); // Переводим в нижний регистр для игнорирования регистра

        for (int i = words.Count - 1; i >= 0; i--)
        {
            if (words[i].Word.ToLower() == wordToRemoveLower)
            {
                words.RemoveAt(i);
                Console.WriteLine($"Слово '{wordToRemove}' успешно удалено из списка слов.");
            }
        }
    }

    /// <summary>
    /// Удаление всех слов с данным переводом
    /// </summary>
    /// <param name="translationToRemove">перевод по которому будет проводится удаление</param>
    public void RemoveAllWordsForTranslation(string translationToRemove)
    {
        var translationToRemoveLower = translationToRemove.ToLower(); // Переводим в нижний регистр для игнорирования регистра

        for (int i = words.Count - 1; i >= 0; i--)
        {
            if (words[i].Translation.ToLower() == translationToRemoveLower)
            {
                words.RemoveAt(i);
                Console.WriteLine($"Слова с переводом '{translationToRemove}' успешно удалены из списка слов.");
            }
        }
    }

    /// <summary>
    /// Удаляет все слова из словаря
    /// </summary>
    public void RemoveAllWords()
    {
        this.words.Clear();
    }

    /// <summary>
    /// Тест по словарю
    /// </summary>
    public void StartTest()
    {
        if (IsEmpty())
        {
            Console.WriteLine("Словарь пуст");
            return;
        }

        foreach (var word in words)
        {
            Console.WriteLine($"Какой перевод данного слова -> '{word.Word}'?");
            string answer = Console.ReadLine();

            if (answer.ToLower() == word.Translation.ToLower())
            {
                Console.WriteLine("Првильно!");
            }
            else
            {
                Console.WriteLine("Неправильно. Правильный перевод этого слова: " + word.Translation);
            }
        }
    }
}