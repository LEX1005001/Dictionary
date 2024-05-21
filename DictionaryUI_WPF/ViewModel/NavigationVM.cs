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

        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnProperetyChanged(); }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand DictionaryCommand { get; set; }
        public ICommand AddWordCommand { get; set; }
        public ICommand DeleteWordCommand { get; set; }
        public ICommand TestCommand { get; set;}


        private void Home(object obj) => CurrentView = new HomeViewModel();
        private void Dictionary(object obj) => CurrentView = new DictionaryViewModel();
        private void AddWord(object obj) => CurrentView = new AddWordViewModel();
        private void DeleteWord(object obj) => CurrentView = new DeleteWordViewModel();
        private void Test(object obj) => CurrentView = new TestViewModel();

        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            DictionaryCommand = new RelayCommand(Dictionary);
            AddWordCommand= new RelayCommand(AddWord);
            DeleteWordCommand= new RelayCommand(DeleteWord);
            TestCommand= new RelayCommand(Test);

            //Strat Page
            CurrentView = new HomeViewModel();
         
        }
    }
}
