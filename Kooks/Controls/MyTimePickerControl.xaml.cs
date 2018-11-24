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
    /// Logika interakcji dla klasy UserControl1.xaml
    /// </summary>
    public partial class MyTimePickerControl : UserControl
    {
        public MyTimePickerControl()
        {
            InitializeComponent();
        }
        byte seconds;
        byte minutes;
        byte hours;

        // hours dependency property
        public string HoursInitialState
        {
            get { return (string)GetValue(HoursInitialStateProperty); }
            set { SetValue(HoursInitialStateProperty, value); }
        }

        public static readonly DependencyProperty HoursInitialStateProperty =
            DependencyProperty.Register("HoursInitialState", typeof(string), typeof(MyTimePickerControl), new UIPropertyMetadata(new PropertyChangedCallback(HoursInitialStateDisplay)));
       
        // minutes dependency property

        public string MinutesInitialState
        {
            get { return (string)GetValue(MinutesInitialStateProperty); }
            set { SetValue(MinutesInitialStateProperty, value); }
        }

        public static readonly DependencyProperty MinutesInitialStateProperty =
            DependencyProperty.Register("MinutesInitialState", typeof(string), typeof(MyTimePickerControl), new UIPropertyMetadata(new PropertyChangedCallback(MinutesInitialStateDisplay)));

        // seconds dependency property

        public string SecondsInitialState
        {
            get { return (string)GetValue(SecondsInitialStateProperty); }
            set { SetValue(SecondsInitialStateProperty, value); }
        }

        public static readonly DependencyProperty SecondsInitialStateProperty =
            DependencyProperty.Register("SecondsInitialState", typeof(string), typeof(MyTimePickerControl), new UIPropertyMetadata(new PropertyChangedCallback(SecondsInitialStateDisplay)));


        private static void HoursInitialStateDisplay(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MyTimePickerControl control = (MyTimePickerControl)d;
            control.HoursTextBox.Text = control.HoursInitialState;

        }
        private static void MinutesInitialStateDisplay(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MyTimePickerControl control = (MyTimePickerControl)d;
            control.MinutesTextBox.Text = control.MinutesInitialState;

        }
        private static void SecondsInitialStateDisplay(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MyTimePickerControl control = (MyTimePickerControl)d;
            control.SecondsTextBox.Text = control.SecondsInitialState;

        }


        private void UpSecondsButton_Click(object sender, RoutedEventArgs e)
        {
            seconds = Byte.Parse(SecondsTextBox.Text);

            if (seconds + 1 <= 60)
            {
                seconds++;
                SecondsTextBox.Text = seconds.ToString();
            }
        }

        private void UpMinutesButton_Click(object sender, RoutedEventArgs e)
        {
            minutes = Byte.Parse(MinutesTextBox.Text);

            if (minutes + 1 <= 60)
            {
                minutes++;
                MinutesTextBox.Text = minutes.ToString();
            }

        }

        private void UpHoursButton_Click(object sender, RoutedEventArgs e)
        {
            hours = Byte.Parse(HoursTextBox.Text);

            if (hours + 1 <= 60)
            {
                hours++;
                HoursTextBox.Text = hours.ToString();
            }
        }

        // downbuttons - tu moznaby region ustalic

        private void DownSecondsButton_Click(object sender, RoutedEventArgs e)
        {
            seconds = Byte.Parse(SecondsTextBox.Text);

            if (seconds - 1 >= 0 && seconds - 1 <= 60)
            {
                seconds--;
                SecondsTextBox.Text = seconds.ToString();
            }
        }

        private void DownMinutesButton_Click(object sender, RoutedEventArgs e)
        {
            minutes = Byte.Parse(MinutesTextBox.Text);

            if (minutes - 1 >= 0 && minutes - 1 <= 60)
            {
                minutes--;
                MinutesTextBox.Text = minutes.ToString();
            }
        }

        private void DownHoursButton_Click(object sender, RoutedEventArgs e)
        {
            hours = Byte.Parse(HoursTextBox.Text);

            if (hours - 1 >= 0 && hours - 1 <= 24)
            {
                hours--;
                HoursTextBox.Text = hours.ToString();
            }
        }
    }
}
