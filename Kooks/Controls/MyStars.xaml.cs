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
    /// Logika interakcji dla klasy MyStars.xaml
    /// </summary>
    public partial class MyStars : UserControl
    {
        public MyStars()
        {
            InitializeComponent();
        }

        public byte Rating
        {
            get { return (byte)GetValue(RatingProperty); }
            set { SetValue(RatingProperty, value); }
        }

        public static readonly DependencyProperty RatingProperty =
            DependencyProperty.Register("Rating", typeof(byte), typeof(MyStars), new UIPropertyMetadata(new PropertyChangedCallback(RatingDisplay)));

        private static void RatingDisplay(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MyStars control = (MyStars)d;

            object[] starsArray = new object[] { control.Star0, control.Star1, control.Star2, control.Star3, control.Star4 };

            Style newStyle = control.FindResource("StarButtonStyle") as Style;

            for (int i = 0; i < control.Rating; i++)
            {
                Control star = (Control)starsArray[i];
                star.Style = newStyle;
            }
        }

    }
}
