﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryUI_WPF.Model
{
    class PageModel
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string Translation { get; set; }
        public string Theme { get; internal set; }
        public string ThemeOfWord { get; internal set; }
        public string TranslationOfNewWord { get; internal set; }
    }
}
