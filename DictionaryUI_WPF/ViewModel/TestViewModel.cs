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

        //1_Выбор темы на которую будет проходиться тест
        //2_Тест формата {Какой перевод у слова isecream} и ввод {мороженое}
        //2.1_Можно сделать обратный формат Перевод -> Слово


        public TestViewModel()
        {
            _pageModel = new PageModel();
        }

    }
}
