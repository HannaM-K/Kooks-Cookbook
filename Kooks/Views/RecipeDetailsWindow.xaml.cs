using Kooks.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
    /// Logika interakcji dla klasy RecipeDetailsWindow.xaml
    /// </summary>
    public partial class RecipeDetailsWindow : Window
    {
        public RecipeDetailsWindow()
        {
            InitializeComponent();
            Style = (Style)FindResource(typeof(Window));
        }
        string previousName;

        private void RecipeDetailsWindowButton_Click(object sender, RoutedEventArgs e) // to musze zmienic na kilka zdarzeń moze 
        {
            Button button = sender as Button;
            if ((string)button.Content == "Moje przepisy")
            {
                if (SaveChangesButton.Visibility == Visibility.Hidden)
                {
                    RecipesWindow recipesWindow = new RecipesWindow();

                    recipesWindow.Show();
                    recipesWindow.MainTabControl.SelectedIndex = 0;

                    this.Close();
                }
                else
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
            }
            else if ((string)button.Content == "Przepisy Kooks")
            {
                if (SaveChangesButton.Visibility == Visibility.Hidden)
                {
                    RecipesWindow recipesWindow = new RecipesWindow();

                    recipesWindow.Show();
                    recipesWindow.MainTabControl.SelectedIndex = 1;

                    this.Close();
                }
                else
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
            else if ((string)button.Content == "Edytuj przepis")
            {
                EditRecipeButton.Visibility = Visibility.Collapsed;
                EditRecipeButtonRectangle.Visibility = Visibility.Collapsed;
                previousName = NameTextBox.Text.Trim();
                NameTextBox.IsReadOnly = false;
                NameTextBox.Background = Brushes.White;
                IngredientsTextBox.IsReadOnly = false;
                IngredientsTextBox.Background = Brushes.White;
                DescriptionTextBox.IsReadOnly = false;
                DescriptionTextBox.Background = Brushes.White;

                MyStarsControl.Visibility = Visibility.Hidden;
                MyStarButtonControl.InitialState = MyStarsControl.Rating; ////////
                MyStarButtonControl.Visibility = Visibility.Visible;

                categoryComboBox.MainComboBox.SelectedValue = CategoryTextBlock.Text;
                categoryComboBox.MainComboBox.Text = CategoryTextBlock.Text;
                CategoryTextBlock.Visibility = Visibility.Collapsed;

                myTimePickerControl.HoursInitialState = hoursLabel.Content.ToString();
                myTimePickerControl.MinutesInitialState = minutesLabel.Content.ToString();
                myTimePickerControl.SecondsInitialState = secondsLabel.Content.ToString();
                myTimePickerControl.Visibility = Visibility.Visible;

                TimeStackPanel.Visibility = Visibility.Hidden;
                categoryComboBox.Visibility = Visibility.Visible;

                AddChangeImageButton.Visibility = Visibility.Visible;
                SaveChangesButton.Visibility = Visibility.Visible;
            }
        }

        private void AddChangeImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Wybierz zdjęcie";

            if (openFileDialog.ShowDialog() == true)
            {
                Console.WriteLine(openFileDialog.FileName);
                CategoryImage.Source = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.RelativeOrAbsolute));
            }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {

            if (categoryComboBox.MainComboBox.SelectedIndex == -1 || NameTextBox.Text.Trim() == "" || DescriptionTextBox.Text.Trim() == "" || IngredientsTextBox.Text.Trim() == "" || MyStarButtonControl.RatingProperty == 0 || (myTimePickerControl.HoursTextBox.Text == "0" && myTimePickerControl.MinutesTextBox.Text == "0" && myTimePickerControl.SecondsTextBox.Text == "0"))
            {
                MessageBox.Show("Uzupełnij wszystkie pola przed zapisem!", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var imageBuffer = BitmapSourceToByteArray((BitmapSource)CategoryImage.Source);
                var timeBuffer = StringToTimeSpan(myTimePickerControl.HoursTextBox.Text, myTimePickerControl.MinutesTextBox.Text, myTimePickerControl.SecondsTextBox.Text);

                using (KooksDataBaseEntities context = new KooksDataBaseEntities())
                {
                    try
                    {
                        var recipe = (from item in context.MyRecipes where item.Name == previousName select item).ToArray();

                        MyRecipe recipeToUpdate = context.MyRecipes.Find(recipe[0].RecipeID);

                        recipeToUpdate.Name = NameTextBox.Text.Trim();
                        recipeToUpdate.Category = categoryComboBox.MainComboBox.Text.Trim();
                        recipeToUpdate.Description = DescriptionTextBox.Text.Trim();
                        recipeToUpdate.Ingredients = IngredientsTextBox.Text.Trim();
                        recipeToUpdate.Level = MyStarButtonControl.RatingProperty;
                        recipeToUpdate.Time = timeBuffer;
                        recipeToUpdate.Image = imageBuffer;

                        context.SaveChanges();
                        MessageBox.Show("Pomyślnie zapisano", "", MessageBoxButton.OK, MessageBoxImage.Information);

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

    }
}
