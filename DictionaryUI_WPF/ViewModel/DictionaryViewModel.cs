using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DictionaryUI_WPF.Model;
using DictionaryClassLibrary;
namespace DictionaryUI_WPF.ViewModel
{
    class DictionaryViewModel : Utilites.ViewModelBase
    {
        private readonly PageModel _pageModel;




        public DictionaryViewModel()
        {
            _pageModel = new PageModel();
        }

    }
}