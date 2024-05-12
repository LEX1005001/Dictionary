using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryUI_WPF.Model
{
    public class Theme
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Word
    {
        public int Id { get; set; }
        public string ThisWord { get; set; }
    }

    public class WordDictionary
    {
        public int Id { get; set; }
        public int ThemeId { get; set; }
        public int WordId { get; set; }
        public string Translation { get; set; }
    }
}
