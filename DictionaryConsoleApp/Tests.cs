using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DictionaryClassLibrary;
using Timelon.Data;

namespace DictionaryConsoleApp
{
    internal class Tests
    {
        static void Main(string[] args)
        {
            //Создаём словарь dictionary1
            DictionaryWords dictionary1 = new DictionaryWords();
            dictionary1.Theme = "Theme 1";              //Даём ему тему Theme 1
            Console.WriteLine($"Кол-во слов в словаре ->{dictionary1.GetSize()}");   //Выводим размер словаря
            dictionary1.StartTest();                    //Пытаемя запустить тест по пустому словарю, ожидаем исключение т.к словарь пуст
            dictionary1.AddWord("chiken", "курица");    //Добавляем в словарь несколько слов
            dictionary1.AddWord("dog", "собака");
            Console.WriteLine($"Кол-во слов в обн. словаре ->{dictionary1.GetSize()}"); //Ещё раз выводим размер обновлённого словаря
            dictionary1.StartTest();                    //Пытаемся запустить тест

            DictionaryWords dictionary2 = new DictionaryWords(); //Создаём словарь dictionary2
            dictionary2.Theme = "Theme 2";                       //Даём ему тему Theme 2
            

            List<DictionaryWords> libraries = new List<DictionaryWords>//Сохраняем все словари в библиотеку libraries
            {
            dictionary1,
            dictionary2
            };

            List<DictionaryWords> themes = DictionaryWords.GetAllThemesFromLibrary(libraries); //Получаем все темы данной библиотеки
            foreach (var theme in themes)
            {
                Console.WriteLine("Theme: " + theme.Theme);
            }

            List<Word_Translation> wordsWithTheme = dictionary1.GetWordsByTheme("Theme 1");//Выводим все слова связанные с этой темой


            foreach (var word in wordsWithTheme)//Выводим все слова связанные с этой темой в данной беблиотеке (Отдельная функция !?)
            {

                Console.WriteLine($"Word in Theme: {word.Word}, Translation: {word.Translation}");

            }
            
            DictionaryWords dictionary3 = DictionaryWords.LoadDictionaryFromXml("Animals.xml"); //Создаём новый словарик и загружаем в него словарь из Animals.xml 
            
            dictionary3.Info(); //Выводим его содержимое
            DictionaryWords.AddDictionaryToList(libraries, dictionary3); //Добавляем новый словарь в библиотеку
            List<DictionaryWords> themes1 = DictionaryWords.GetAllThemesFromLibrary(libraries);//Сохраняем темы новой библиотеки
            foreach (var theme in themes1)
            {
                Console.WriteLine("Тема: "+theme.Theme);    //Выводим их и видим что добавление прошло успешно
            }


            Console.ReadKey();
            

            
            
        }


    }
    
}
