using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DictionaryUI_WPF.Model;
using DictionaryClassLibrary;
using System.Windows.Controls;

namespace DictionaryUI_WPF.ViewModel
{
    class DictionaryViewModel : Utilites.ViewModelBase
    {
        private readonly PageModel _pageModel;
        

        //1_получение список Тем слов :(animal,food,street)
        //2_после выбора темы выводить все слова связанные с данной темой

        /*private void ThemesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedTheme = (themesListBox.SelectedItem as ListBoxItem)?.Content.ToString();

            if (selectedTheme != null)
            {
                wordsListView.Items.Clear();
                foreach (string word in themeWords[selectedTheme])
                {
                    wordsListView.Items.Add(new ListViewItem { Content = word });
                }
            }
        }*/

        public DictionaryViewModel()
        {
            _pageModel = new PageModel();
        }

    }
}