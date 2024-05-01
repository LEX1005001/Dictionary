using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DictionaryUI_WPF.Model;
using DictionaryClassLibrary;

namespace DictionaryUI_WPF.ViewModel
{
    class DeleteWordViewModel:Utilites.ViewModelBase
    {
        private readonly PageModel _pageModel;

        public int NumberId2Delete
        {
            get { return _pageModel.Id; }
            set { _pageModel.Id = value; OnProperetyChanged(); }
        }


        public DeleteWordViewModel()
        {
            _pageModel = new PageModel();
            NumberId2Delete = 404;
        }
    }
}
