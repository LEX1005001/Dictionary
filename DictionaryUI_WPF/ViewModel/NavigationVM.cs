using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DictionaryUI_WPF.Model;
using System.Windows.Input;
using System.Runtime.InteropServices;
using DictionaryUI_WPF.Utilites;

namespace DictionaryUI_WPF.ViewModel
{
    class NavigationVM:Utilites.ViewModelBase
    {
        private object _currentView;

        private object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnProperetyChanged(); }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand DictionaryCommand { get; set; }
        public ICommand ChangeWordCommand { get; set; }
        public ICommand TestCommand { get; set;}


        private void Home(object obj) => CurrentView = new HomeViewModel();
        private void Dictionary(object obj) => CurrentView = new DictionaryViewModel();
        private void ChangeWord(object obj) => CurrentView = new ChangeWordViewModel();
        private void Test(object obj) => CurrentView = new TestViewModel();

        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            DictionaryCommand = new RelayCommand(Dictionary);
            ChangeWordCommand= new RelayCommand(ChangeWord);
            TestCommand= new RelayCommand(Test);

            //Strat Page
            CurrentView = new HomeViewModel();
        }


    }
}
