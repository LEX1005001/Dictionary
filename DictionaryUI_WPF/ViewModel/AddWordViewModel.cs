using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DictionaryUI_WPF.Model;
using DictionaryClassLibrary;


namespace DictionaryUI_WPF.ViewModel
{
    class AddWordViewModel : Utilites.ViewModelBase
    {
        private readonly PageModel _pageModel;


        public string NewWord
        {
            get { return _pageModel.Word; }
            set { _pageModel.Word = value; OnProperetyChanged(); }

        }

        public string TranslationOfNewWord
        {
            get { return _pageModel.TranslationOfNewWord; }
            set { _pageModel.TranslationOfNewWord = value; OnProperetyChanged(); }
        }

        public string ThemeOfWord
        {
            get { return _pageModel.ThemeOfWord; }
            set { _pageModel.ThemeOfWord = value; OnProperetyChanged(); }

        }

        public AddWordViewModel()
        {
            _pageModel = new PageModel();
            NewWord = string.Empty;
            TranslationOfNewWord = string.Empty;
            ThemeOfWord = string.Empty;

        }
    }
}
