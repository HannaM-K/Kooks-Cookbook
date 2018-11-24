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
    /// Logika interakcji dla klasy MyCategoryComboBox.xaml
    /// </summary>
    public partial class MyCategoryComboBox : UserControl
    {
        public MyCategoryComboBox()
        {
            InitializeComponent();
        }
        //public string TextValue
        //{
        //    get { return (string)GetValue(TextProperty); }
        //    set { SetValue(TextProperty, value); }
        //}

        //public static readonly DependencyProperty TextProperty =
        //    DependencyProperty.Register("TextValue", typeof(string), typeof(MyCategoryComboBox), new UIPropertyMetadata(new PropertyChangedCallback(TextDisplay)));

        //private static void TextDisplay(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    MyCategoryComboBox control = (MyCategoryComboBox)d;
        //    control.MainComboBox.Text = control.TextValue;
          
        //}
    }
}
