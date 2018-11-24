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

namespace Kooks.Controls
{
    /// <summary>
    /// Logika interakcji dla klasy MyMenuControl.xaml
    /// </summary>
    public partial class MyMenuControl : UserControl
    {
        public MyMenuControl()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if ((string)menuItem.Header == "Zamknij")
            {
                Application.Current.Shutdown();
            }
            else
            {
                MessageBox.Show("Info");
            }
        }
    }
}
