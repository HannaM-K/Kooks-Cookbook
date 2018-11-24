using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;


namespace Test
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DisplayByteArray();

        }

        void DisplayByteArray()
        {
            BitmapSource imgSource = (BitmapSource)img.Source;
            byte[] imageBuffer = BitmapSourceToByteArray(imgSource);

            string bytes = "";

            foreach (var item in imageBuffer)
            {
                bytes += item;
            }

            display.Content = bytes;

            using (var tw = new StreamWriter(@"C:\Users\Hania\Desktop\new.txt"))
            {
                tw.Write(bytes);
            }
        }

        private byte[] BitmapSourceToByteArray(BitmapSource image)
        {
            using (var stream = new MemoryStream())
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(stream);
                return stream.ToArray();
            }
        }
    }
}
