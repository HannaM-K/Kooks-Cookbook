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

namespace Kooks.Controls
{
    /// <summary>
    /// Logika interakcji dla klasy MyRecipeControl.xaml
    /// </summary>
    public partial class MyRecipeControl : UserControl
    {
        public MyRecipeControl()
        {
            InitializeComponent();
        }
        bool clicked;
        public bool Clicked
        {
            get {return clicked; }
        }

        SolidColorBrush greenBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#23ef67"));
        SolidColorBrush whiteBrush = new SolidColorBrush(Colors.White);

        public byte Rating
        {
            get { return (byte)GetValue(RatingProperty); }
            set { SetValue(RatingProperty, value); }
        }

        public static readonly DependencyProperty RatingProperty =
            DependencyProperty.Register("Rating", typeof(byte), typeof(MyRecipeControl), new UIPropertyMetadata(new PropertyChangedCallback(RatingDisplay)));

        private static void RatingDisplay(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MyRecipeControl control = (MyRecipeControl)d;

            object[] starsArray = new object[] { control.Star0, control.Star1, control.Star2, control.Star3, control.Star4 };

            Style newStyle = control.FindResource("SmallStarButtonStyle") as Style;

            for (int i = 0; i < control.Rating; i++)
            {
                Control star = (Control)starsArray[i];
                star.Style = newStyle;
            }
        }
        private void MainStackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            MainBorder.BorderBrush = greenBrush;
        }

        private void MainStackPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            MainBorder.BorderBrush = whiteBrush;
        }

        private void MainStackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (clicked == false)
            {
                MainBorder.BorderBrush = greenBrush;
                MainStackPanel.MouseLeave -= MainStackPanel_MouseLeave;
                clicked = true;
            }
            else
            {
                MainBorder.BorderBrush = whiteBrush;
                MainStackPanel.MouseLeave += MainStackPanel_MouseLeave;
                clicked = false;
            }

        }

    }
}
