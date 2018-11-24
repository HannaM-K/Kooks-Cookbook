using Kooks.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
using System.Windows.Shapes;

namespace Kooks.Views
{
    /// <summary>
    /// Logika interakcji dla klasy AddRecipeWindow.xaml
    /// </summary>
    public partial class AddRecipeWindow : Window
    {
        public AddRecipeWindow()
        {
            InitializeComponent();
            Style = (Style)FindResource(typeof(Window));
        }
        private void AddRecipeWindowBtn_Click(object sender, RoutedEventArgs e) // to jest prawdopodobnie bardzo brzydka nazwa zdarzenia i przycisku
        {

            Button btn = sender as Button;
            if ((string)btn.Content == "Moje przepisy")
            {
                var result = MessageBox.Show("Czy chcesz wyjść bez zapisywania?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    RecipesWindow recipesWindow = new RecipesWindow();
                    recipesWindow.Show();
                    recipesWindow.MainTabControl.SelectedIndex = 0;

                    this.Close();
                }

            }
            else if ((string)btn.Content == "Przepisy Kooks")
            {

                var result = MessageBox.Show("Czy chcesz wyjść bez zapisywania?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    RecipesWindow recipesWindow = new RecipesWindow();
                    recipesWindow.Show();
                    recipesWindow.MainTabControl.SelectedIndex = 1;

                    this.Close();
                }
            }
        }

        private void CategoryComboBox_Selected(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedItem = sender as ComboBoxItem;

            if ((string)selectedItem.Content == "Danie główne")
            {
                RecipeImage.Source = new BitmapImage(new Uri("/Images/Food Categories/icons8-danie-główne-96.png", UriKind.Relative));
            }
            else
            if ((string)selectedItem.Content == "Zupa")
            {
                RecipeImage.Source = new BitmapImage(new Uri("/Images/Food Categories/icons8-zupa-96.png", UriKind.Relative));
            }
            else
            if ((string)selectedItem.Content == "Przystawka")
            {
                RecipeImage.Source = new BitmapImage(new Uri("/Images/Food Categories/icons8-przystawka-96.png", UriKind.Relative));
            }
            else
            if ((string)selectedItem.Content == "Deser")
            {
                RecipeImage.Source = new BitmapImage(new Uri("/Images/Food Categories/icons8-deser-96.png", UriKind.Relative));
            }
            else
            if ((string)selectedItem.Content == "Napój")
            {
                RecipeImage.Source = new BitmapImage(new Uri("/Images/Food Categories/icons8-napój-96.png", UriKind.Relative));
            }
            else
            if ((string)selectedItem.Content == "Pieczywo")
            {
                RecipeImage.Source = new BitmapImage(new Uri("/Images/Food Categories/icons8-pieczywo-96.png", UriKind.Relative));
            }
            else
            if ((string)selectedItem.Content == "Śniadanie")
            {
                RecipeImage.Source = new BitmapImage(new Uri("/Images/Food Categories/icons8-śniadanie-96.png", UriKind.Relative));
            }
            else
            if ((string)selectedItem.Content == "Kolacja")
            {
                RecipeImage.Source = new BitmapImage(new Uri("/Images/Food Categories/icons8-kolacja-96.png", UriKind.Relative));
            }
            else
            if ((string)selectedItem.Content == "Inne")
            {
                RecipeImage.Source = new BitmapImage(new Uri("/Images/Food Categories/icons8-inne-96.png", UriKind.Relative));
            }
        }

        private void AddChangeImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Wybierz zdjęcie";

            if (openFileDialog.ShowDialog() == true)
            {
                Console.WriteLine(openFileDialog.FileName);
                RecipeImage.Source = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.RelativeOrAbsolute));
            }
            Style newStyle = FindResource("TriangleButtonStyle") as Style;
            AddChangeImageButton.Style = newStyle;

            AddChangeImageButton.Width = 35;
            AddChangeImageButton.Height = 34;
            AddChangeImageButton.SetValue(Canvas.LeftProperty, 237.0);
            AddChangeImageButton.SetValue(Canvas.TopProperty, 194.0);


            MainCourse.Selected -= CategoryComboBox_Selected;
            Soup.Selected -= CategoryComboBox_Selected;
            SideDish.Selected -= CategoryComboBox_Selected;
            Supper.Selected -= CategoryComboBox_Selected;
            Dessert.Selected -= CategoryComboBox_Selected;
            Baking.Selected -= CategoryComboBox_Selected;
            Breakfast.Selected -= CategoryComboBox_Selected;
            Other.Selected -= CategoryComboBox_Selected;
            Drink.Selected -= CategoryComboBox_Selected;
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
        private TimeSpan StringToTimeSpan(string hours, string minutes, string seconds)
        {
            return new TimeSpan(int.Parse(hours), int.Parse(minutes), int.Parse(seconds));
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        // moze lepiej zebym nie uzywala tu messageboxow typowych, albo pozbede sie chociaz systemowych dzwiekow.
        //Poza tym musze sie przekonac czy nie chce ich jakichs ladniejszych
        //oraz przy bindowaniu moge zrobic to ze sie czerwona obwodka bedzie robic przy niewypelnionej kontrolce
        {
            MyRecipe[] myRecipe = new MyRecipe[0];
            using (KooksDataBaseEntities context = new KooksDataBaseEntities())
            {
                myRecipe = (from item in context.MyRecipes where item.Name == (string)NameTextBox.Text.Trim() select item).ToArray();
            }
            if (myRecipe.Length != 0)
            {
                MessageBox.Show("Przepis o podanej nazwie już istnieje. Zmień nazwę przepisu.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                if (CategoryComboBox.SelectedValue == null || NameTextBox.Text.Trim() == "" || DescriptionTextBox.Text.Trim() == "" || IngredientsTextBox.Text.Trim() == "" || MyStarButtonControl.RatingProperty == 0 || (MyTimePicker.HoursTextBox.Text == "0" && MyTimePicker.MinutesTextBox.Text == "0" && MyTimePicker.SecondsTextBox.Text == "0"))
                {
                    MessageBox.Show("Uzupełnij wszystkie pola przed zapisem!", "", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                {
                    var imageBuffer = BitmapSourceToByteArray((BitmapSource)RecipeImage.Source);
                    var timeBuffer = StringToTimeSpan(MyTimePicker.HoursTextBox.Text, MyTimePicker.MinutesTextBox.Text, MyTimePicker.SecondsTextBox.Text);

                    var name = NameTextBox.Text.Trim();
                    var category = CategoryComboBox.Text.Trim();
                    var description = DescriptionTextBox.Text.Trim();
                    var ingredients = IngredientsTextBox.Text.Trim();
                    byte level = MyStarButtonControl.RatingProperty;

                    using (KooksDataBaseEntities context = new KooksDataBaseEntities())
                    {
                        try
                        {
                            context.MyRecipes.Add(new MyRecipe() { Name = name, Category = category, Description = description, Ingredients = ingredients, Image = imageBuffer, Level = level, Time = timeBuffer, Creation_Date = DateTime.Now });
                            context.SaveChanges();
                            MessageBox.Show("Pomyślnie zapisano zmiany", "", MessageBoxButton.OK, MessageBoxImage.Information);

                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Błąd zapisu", "", MessageBoxButton.OK, MessageBoxImage.Error);

                        }
                    }
                    RecipesWindow recipesWindow = new RecipesWindow();
                    recipesWindow.Show();
                    this.Close();
                }
            }
        }
    }
}
