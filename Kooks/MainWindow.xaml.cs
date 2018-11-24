using Kooks.ViewModels;
using Kooks.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kooks
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Style = (Style)FindResource(typeof(Window));
        }

        private void MainWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if ((string)btn.Content == "Dodaj przepis")
            {
                AddRecipeWindow addRecipeWindow = new AddRecipeWindow();
                addRecipeWindow.Show();
                this.Close();
            }
            else
            {
                RecipesWindow RecipesWindow = new RecipesWindow();
                RecipesWindow.Show();
                if ((string)btn.Content == "Moje przepisy")
                {
                    RecipesWindow.MainTabControl.SelectedIndex = 0;
                    this.Close();
                }
                else if ((string)btn.Content == "Przepisy Kooks")
                {
                    RecipesWindow.MainTabControl.SelectedIndex = 1;
                    this.Close();
                }
            }

        }

    }
}
