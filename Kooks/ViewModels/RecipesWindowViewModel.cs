using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooks.ViewModels
{
    public class RecipesWindowViewModel
    {
        public ObservableCollection<string> MyRecipesSearchComboBoxContent { get; private set; }
        public ObservableCollection<string> KooksRecipesSearchComboBoxContent { get; private set; }


        public RecipesWindowViewModel()
        {
            MyRecipesSearchComboBoxContent = new ObservableCollection<string> { };
            KooksRecipesSearchComboBoxContent = new ObservableCollection<string> { };

            using (KooksDataBaseEntities context = new KooksDataBaseEntities())
            {
                foreach (var item in context.MyRecipes)
                {
                    MyRecipesSearchComboBoxContent.Add(item.Name);
                }
                foreach (var item in context.KooksRecipes)
                {
                    KooksRecipesSearchComboBoxContent.Add(item.Name);
                }
            };
        }
    }
}
