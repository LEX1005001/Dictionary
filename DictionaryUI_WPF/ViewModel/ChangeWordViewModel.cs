using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DictionaryUI_WPF.Model;
using DictionaryClassLibrary;

namespace DictionaryUI_WPF.ViewModel
{
    class ChangeWordViewModel : Utilites.ViewModelBase
    {
        private readonly PageModel _pageModel;

       
        public string NewWord
        {
            get { return _pageModel.Word; }
            set { _pageModel.Word = value; OnProperetyChanged(); }

        }

        public string Translation
        {
            get { return _pageModel.Translation; }
            set { _pageModel.Translation = value; OnProperetyChanged(); }
        }

        public string Theme
        {
            get { return _pageModel.Theme; }
            set { _pageModel.Theme = value; OnProperetyChanged(); }
        }

        public ChangeWordViewModel()
        {
            _pageModel = new PageModel();

        }
    }
}
