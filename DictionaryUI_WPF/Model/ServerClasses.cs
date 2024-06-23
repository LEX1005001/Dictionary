using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryUI_WPF.Model
{
    public class Word_Tr
    {
        public int Id { get; set; }

        public string Word { get; set; } = string.Empty;

        public string Translation { get; set; } = string.Empty;

        public int ThemeId { get; set; }
    }

    public class Theme
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
