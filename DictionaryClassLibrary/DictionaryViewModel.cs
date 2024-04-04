using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryClassLibrary
{
    /// <summary>
    /// Для будующего интерфеса
    /// </summary>
    public class DictionaryViewModel
    {
        public class DictionaryWordsViewModel
        {
            public string Theme { get; set; }
            public List<Word_Translation> Words { get; set; }

            public DictionaryWordsViewModel(DictionaryWords dictionary)
            {
                Theme = dictionary.Theme;
                Words = dictionary.Words;
            }
        }
    }
}
