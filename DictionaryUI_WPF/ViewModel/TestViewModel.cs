using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DictionaryUI_WPF.Model;

namespace DictionaryUI_WPF.ViewModel
{
    class TestViewModel:Utilites.ViewModelBase
    {
        private readonly PageModel _pageModel;




        public TestViewModel()
        {
            _pageModel = new PageModel();
        }

    }
}
