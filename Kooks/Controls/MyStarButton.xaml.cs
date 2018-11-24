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
    /// Logika interakcji dla klasy MyStarButton.xaml
    /// </summary>
    public partial class MyStarButton : UserControl
    {
        public MyStarButton()
        {
            InitializeComponent();
        }
        private byte rating = 0;
        public byte RatingProperty
        {
            get { return rating; }
        }

        bool[] ratingArray = new bool[5];
        Button[] starsArray;

        public Button[] StarsArray
        {
            get
            {
                starsArray = new Button[] { Star0, Star1, Star2, Star3, Star4 };
                return starsArray;
            }

        }

        public byte InitialState
        {
            get { return (byte)GetValue(InitialStateProperty); }
            set { SetValue(InitialStateProperty, value); }
        }

        public static readonly DependencyProperty InitialStateProperty =
            DependencyProperty.Register("InitialState", typeof(byte), typeof(MyStarButton), new UIPropertyMetadata(new PropertyChangedCallback(InitialStateDisplay)));

        private static void InitialStateDisplay(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MyStarButton control = (MyStarButton)d;

            Style newStyle = control.FindResource("StarButtonStyle") as Style;

            for (int i = 0; i < control.InitialState; i++)
            {
                control.StarsArray[i].Style = newStyle;
            }

            for (int i = 0; i < control.InitialState; i++)
            {
                control.ratingArray[i] = true;
                control.rating++;
            }
        }


        private void Star_MouseEnter(object sender, MouseEventArgs e)
        {
            int MouseEnteredStarIndex = StarStackPanel.Children.IndexOf(sender as UIElement);
            Style newStyle = this.FindResource("GrayStarButtonStyle") as Style;

            for (int i = 0; i < StarsArray.Length; i++)
            {
                StarsArray[i].Style = newStyle;

            }
            newStyle = this.FindResource("BrightStarButtonStyle") as Style;

            for (int i = 0; i <= MouseEnteredStarIndex; i++) // ten index ciagle odnosci sie do 9 elementow!
            {
                StarsArray[i].Style = newStyle;
            }
        }

        private void Star_MouseLeave(object sender, MouseEventArgs e)
        {
            int MouseLeavedStarIndex = StarStackPanel.Children.IndexOf(sender as UIElement);
            Control star = sender as Button;

            Style GrayStarStyle = FindResource("GrayStarButtonStyle") as Style;
            Style GoldenStarStyle = FindResource("StarButtonStyle") as Style;

            for (int i = 0; i < StarsArray.Length; i++)
            {
                if (ratingArray[i] == false)
                {
                    StarsArray[i].Style = GrayStarStyle;
                }
                if (ratingArray[i] == true)
                {
                    StarsArray[i].Style = GoldenStarStyle;
                }
            }

        }

        private void Star_Click(object sender, RoutedEventArgs e)
        {
            rating = 0;
            for (int i = 0; i < StarsArray.Length; i++)
            {
                ratingArray[i] = false;
            }

            int MouseClickedStarIndex = StarStackPanel.Children.IndexOf(sender as UIElement);

            Style newStyle = this.FindResource("StarButtonStyle") as Style;

            for (int i = 0; i <= MouseClickedStarIndex; i++)
            {
                StarsArray[i].Style = newStyle;
                ratingArray[i] = true;
                rating++;
            }
        }
    }
}
