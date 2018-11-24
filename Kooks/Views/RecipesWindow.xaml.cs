using Kooks.Views;
using Kooks.ViewModels;
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
using System.Windows.Shapes;
using Kooks.Controls;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;

namespace Kooks
{
    /// <summary>
    /// Logika interakcji dla klasy MyRecipesWindow.xaml
    /// </summary>
    public partial class RecipesWindow : Window
    {
        public RecipesWindow()
        {
            InitializeComponent();
            DataContext = new RecipesWindowViewModel();
            LoadMyRecipes();
            LoadKooksRecipes();
            Style = (Style)FindResource(typeof(Window));

        }
        MyRecipe[] myRecipesArray;
        KooksRecipe[] kooksRecipesArray;



        private void SortRecipes()
        {
            ComboBox comboBox = new ComboBox();

            if (MainTabControl.SelectedIndex == 0)
            {
                comboBox = SortingMyRecipesComboBox;
            }
            else if (MainTabControl.SelectedIndex == 1)
            {
                comboBox = SortingKooksRecipesComboBox;
            }

            if (comboBox.Text != null)
            {
                if (comboBox.Text == "Nazwy rosnąco") // powinnam tu sie jednak odwolywac do nazw, zeby to bylo bardziej internacjonalne, bo to tutaj tylko polak zrozumie -.-
                {
                    AscendingNameSort();
                }
                else if (comboBox.Text == "Nazwy malejąco")
                {
                    DescendingNameSort();
                }
                else if (comboBox.Text == "Daty utworzenia rosnąco")
                {
                    AscendingCreationDateSort();
                }
                else if (comboBox.Text == "Daty utworzenia malejąco")
                {
                    DescendingCreationDateSort();
                }
                else if (comboBox.Text == "Poziomu trudności rosnąco")
                {
                    AscendingLevelSort();
                }
                else if (comboBox.Text == "Poziomu trudności malejąco")
                {
                    DescendingLevelSort();
                }
                else if (comboBox.Text == "Czasu wykonania rosnąco")
                {
                    AscendingTimeSort();
                }
                else if (comboBox.Text == "Czasu wykonania malejąco")
                {
                    DescendingTimeSort();
                }
            }
        }

        private void LoadMyRecipes()
        {
            MyRecipesGrid.Children.Clear();

            using (KooksDataBaseEntities context = new KooksDataBaseEntities())
            {
                int row = 0;

                myRecipesArray = (from item in context.MyRecipes select item).ToArray();

                for (int i = 0; i < myRecipesArray.Length; i++)
                {
                    MyRecipesGrid.RowDefinitions.Add(new RowDefinition());
                    for (int column = 0; column < 4; column++)
                    {
                        ColumnDefinition columnDefinition = new ColumnDefinition();
                        columnDefinition.Width = new GridLength(197.8);
                        MyRecipesGrid.ColumnDefinitions.Add(columnDefinition);

                        MyRecipeControl myRecipeControl = new MyRecipeControl();

                        myRecipeControl.NameLabel.Content = myRecipesArray[i].Name;
                        myRecipeControl.RecipeImage.Source = ConvertByteArrayToBitmapImage(myRecipesArray[i].Image);
                        myRecipeControl.HoursLabel.Content = myRecipesArray[i].Time.Hours.ToString();
                        myRecipeControl.MinutesLabel.Content = myRecipesArray[i].Time.Minutes.ToString();
                        myRecipeControl.SecondsLabel.Content = myRecipesArray[i].Time.Seconds.ToString();
                        myRecipeControl.Rating = myRecipesArray[i].Level;
                        myRecipeControl.MouseDoubleClick += MyRecipeControl_MouseDoubleClick;

                        Grid.SetColumn(myRecipeControl, column);
                        Grid.SetRow(myRecipeControl, row);
                        MyRecipesGrid.Children.Add(myRecipeControl);

                        if (myRecipesArray[i] == myRecipesArray.Last())
                        {
                            break;
                        }
                        if (column < 3)
                        {
                            i++;
                        }
                    }
                    row++;
                }

            }
            SortRecipes();

        }
        private void MyRecipeControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MyRecipeControl myControl = (MyRecipeControl)sender;
            string recipeName = myControl.NameLabel.Content.ToString();

            using (KooksDataBaseEntities context = new KooksDataBaseEntities())
            {
                RecipeDetailsWindow recipeDetailsWindow = new RecipeDetailsWindow();

                if (MainTabControl.SelectedIndex == 0)
                {
                    var myRecipe = (from item in context.MyRecipes where item.Name == recipeName select item).ToArray();

                    recipeDetailsWindow.NameTextBox.Text = recipeName;
                    recipeDetailsWindow.IngredientsTextBox.Text = myRecipe[0].Ingredients;
                    recipeDetailsWindow.DescriptionTextBox.Text = myRecipe[0].Description;
                    recipeDetailsWindow.CategoryTextBlock.Text = myRecipe[0].Category;
                    recipeDetailsWindow.CategoryImage.Source = ConvertByteArrayToBitmapImage(myRecipe[0].Image);
                    recipeDetailsWindow.hoursLabel.Content = myRecipe[0].Time.Hours.ToString();
                    recipeDetailsWindow.minutesLabel.Content = myRecipe[0].Time.Minutes.ToString();
                    recipeDetailsWindow.secondsLabel.Content = myRecipe[0].Time.Seconds.ToString();
                    recipeDetailsWindow.MyStarsControl.Rating = myRecipe[0].Level;
                }
                else if (MainTabControl.SelectedIndex == 1)
                {
                    var kooksRecipe = (from item in context.KooksRecipes where item.Name == recipeName select item).ToArray();

                    recipeDetailsWindow.NameTextBox.Text = recipeName;
                    recipeDetailsWindow.IngredientsTextBox.Text = kooksRecipe[0].Ingredients;
                    recipeDetailsWindow.DescriptionTextBox.Text = kooksRecipe[0].Description;
                    recipeDetailsWindow.CategoryTextBlock.Text = kooksRecipe[0].Category;
                    recipeDetailsWindow.CategoryImage.Source = ConvertByteArrayToBitmapImage(kooksRecipe[0].Image);
                    recipeDetailsWindow.hoursLabel.Content = kooksRecipe[0].Time.Hours.ToString();
                    recipeDetailsWindow.minutesLabel.Content = kooksRecipe[0].Time.Minutes.ToString();
                    recipeDetailsWindow.secondsLabel.Content = kooksRecipe[0].Time.Seconds.ToString();
                    recipeDetailsWindow.MyStarsControl.Rating = kooksRecipe[0].Level;

                    recipeDetailsWindow.EditRecipeButton.Visibility = Visibility.Collapsed;
                }
                recipeDetailsWindow.Show();
            }
            this.Close();
        }

        private void LoadKooksRecipes()
        {

            KooksRecipesGrid.Children.Clear();

            using (KooksDataBaseEntities context = new KooksDataBaseEntities())
            {
                int row = 0;

                kooksRecipesArray = (from item in context.KooksRecipes select item).ToArray();

                for (int i = 0; i < kooksRecipesArray.Length; i++)
                {
                    KooksRecipesGrid.RowDefinitions.Add(new RowDefinition());
                    for (int column = 0; column < 4; column++)
                    {
                        ColumnDefinition columnDefinition = new ColumnDefinition();
                        columnDefinition.Width = new GridLength(197.8);
                        KooksRecipesGrid.ColumnDefinitions.Add(columnDefinition);

                        MyRecipeControl myRecipeControl = new MyRecipeControl();

                        myRecipeControl.NameLabel.Content = kooksRecipesArray[i].Name;
                        myRecipeControl.RecipeImage.Source = ConvertByteArrayToBitmapImage(kooksRecipesArray[i].Image);
                        myRecipeControl.HoursLabel.Content = kooksRecipesArray[i].Time.Hours.ToString();
                        myRecipeControl.MinutesLabel.Content = kooksRecipesArray[i].Time.Minutes.ToString();
                        myRecipeControl.SecondsLabel.Content = kooksRecipesArray[i].Time.Seconds.ToString();
                        myRecipeControl.Rating = kooksRecipesArray[i].Level;
                        myRecipeControl.MouseDoubleClick += MyRecipeControl_MouseDoubleClick;

                        Grid.SetColumn(myRecipeControl, column);
                        Grid.SetRow(myRecipeControl, row);
                        KooksRecipesGrid.Children.Add(myRecipeControl);

                        if (kooksRecipesArray[i] == kooksRecipesArray.Last())
                        {
                            break;
                        }
                        if (column < 3)
                        {
                            i++;
                        }
                    }
                    row++;
                }
            }
            SortRecipes();

        }

        private BitmapImage ConvertByteArrayToBitmapImage(Byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            stream.Seek(0, SeekOrigin.Begin);
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }

        private void RecipesWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if ((string)btn.Content == "Dodaj przepis")
            {
                AddRecipeWindow addRecipeWindow = new AddRecipeWindow();
                addRecipeWindow.Show();
                this.Close();
            }
        }

        private void AllCategories_Click(object sender, RoutedEventArgs e)
        {
            if (MainTabControl.SelectedIndex == 0)
            {
                LoadMyRecipes();
            }
            else
            {
                LoadKooksRecipes();
            }
        }

        private void MyRecipesCategoryFilter(string filter)
        {
            using (KooksDataBaseEntities context = new KooksDataBaseEntities())
            {
                int row = 0;

                myRecipesArray = (from item in context.MyRecipes where item.Category == filter select item).ToArray();

                MyRecipesGrid.Children.Clear();

                for (int i = 0; i < myRecipesArray.Length; i++)
                {
                    MyRecipesGrid.RowDefinitions.Add(new RowDefinition());
                    for (int column = 0; column < 4; column++)
                    {
                        ColumnDefinition columnDefinition = new ColumnDefinition();
                        columnDefinition.Width = new GridLength(197.8);
                        MyRecipesGrid.ColumnDefinitions.Add(columnDefinition);

                        MyRecipeControl myRecipeControl = new MyRecipeControl();

                        myRecipeControl.NameLabel.Content = myRecipesArray[i].Name;
                        myRecipeControl.RecipeImage.Source = ConvertByteArrayToBitmapImage(myRecipesArray[i].Image);
                        myRecipeControl.HoursLabel.Content = myRecipesArray[i].Time.Hours.ToString();
                        myRecipeControl.MinutesLabel.Content = myRecipesArray[i].Time.Minutes.ToString();
                        myRecipeControl.SecondsLabel.Content = myRecipesArray[i].Time.Seconds.ToString();
                        myRecipeControl.Rating = myRecipesArray[i].Level;
                        myRecipeControl.MouseDoubleClick += MyRecipeControl_MouseDoubleClick;


                        Grid.SetColumn(myRecipeControl, column);
                        Grid.SetRow(myRecipeControl, row);
                        MyRecipesGrid.Children.Add(myRecipeControl);
                        if (myRecipesArray[i] == myRecipesArray.Last())
                        {
                            break;
                        }
                        if (column < 3)
                        {
                            i++;
                        }
                    }
                    row++;
                }
            }
            SortRecipes();
        }

        private void KooksRecipesCategoryFilter(string filter)
        {
            using (KooksDataBaseEntities context = new KooksDataBaseEntities())
            {
                int row = 0;

                kooksRecipesArray = (from item in context.KooksRecipes where item.Category == filter select item).ToArray();

                KooksRecipesGrid.Children.Clear();

                for (int i = 0; i < kooksRecipesArray.Length; i++)
                {
                    KooksRecipesGrid.RowDefinitions.Add(new RowDefinition());
                    for (int column = 0; column < 4; column++)
                    {
                        ColumnDefinition columnDefinition = new ColumnDefinition();
                        columnDefinition.Width = new GridLength(197.8);
                        KooksRecipesGrid.ColumnDefinitions.Add(columnDefinition);

                        MyRecipeControl myRecipeControl = new MyRecipeControl();

                        myRecipeControl.NameLabel.Content = kooksRecipesArray[i].Name;
                        myRecipeControl.RecipeImage.Source = ConvertByteArrayToBitmapImage(kooksRecipesArray[i].Image);
                        myRecipeControl.HoursLabel.Content = kooksRecipesArray[i].Time.Hours.ToString();
                        myRecipeControl.MinutesLabel.Content = kooksRecipesArray[i].Time.Minutes.ToString();
                        myRecipeControl.SecondsLabel.Content = kooksRecipesArray[i].Time.Seconds.ToString();
                        myRecipeControl.Rating = kooksRecipesArray[i].Level;
                        myRecipeControl.MouseDoubleClick += MyRecipeControl_MouseDoubleClick;

                        Grid.SetColumn(myRecipeControl, column);
                        Grid.SetRow(myRecipeControl, row);
                        KooksRecipesGrid.Children.Add(myRecipeControl);

                        if (kooksRecipesArray[i] == kooksRecipesArray.Last())
                        {
                            break;
                        }
                        if (column < 3)
                        {
                            i++;
                        }
                    }
                    row++;
                }
                SortRecipes();

            }
        }

        private void SideDishes_Click(object sender, RoutedEventArgs e)
        {
            if (MainTabControl.SelectedIndex == 0)
            {
                MyRecipesCategoryFilter("Przystawka");
            }
            else
            {
                KooksRecipesCategoryFilter("Przystawka");
            }
        }

        private void Soups_Click(object sender, RoutedEventArgs e)
        {
            if (MainTabControl.SelectedIndex == 0)
            {
                MyRecipesCategoryFilter("Zupa");
            }
            else
            {
                KooksRecipesCategoryFilter("Zupa");
            }
        }

        private void MainCourses_Click(object sender, RoutedEventArgs e)
        {
            if (MainTabControl.SelectedIndex == 0)
            {
                MyRecipesCategoryFilter("Danie główne");
            }
            else
            {
                KooksRecipesCategoryFilter("Danie główne");
            }
        }

        private void Desserts_Click(object sender, RoutedEventArgs e)
        {
            if (MainTabControl.SelectedIndex == 0)
            {
                MyRecipesCategoryFilter("Deser");
            }
            else
            {
                KooksRecipesCategoryFilter("Deser");
            }
        }

        private void Suppers_Click(object sender, RoutedEventArgs e)
        {
            if (MainTabControl.SelectedIndex == 0)
            {
                MyRecipesCategoryFilter("Kolacja");
            }
            else
            {
                KooksRecipesCategoryFilter("Kolacja");
            }
        }

        private void Drinks_Click(object sender, RoutedEventArgs e)
        {
            if (MainTabControl.SelectedIndex == 0)
            {
                MyRecipesCategoryFilter("Napój");
            }
            else
            {
                KooksRecipesCategoryFilter("Napój");
            }
        }

        private void Breakfasts_Click(object sender, RoutedEventArgs e)
        {
            if (MainTabControl.SelectedIndex == 0)
            {
                MyRecipesCategoryFilter("Śniadanie");
            }
            else
            {
                KooksRecipesCategoryFilter("Śniadanie");
            }
        }

        private void Bakings_Click(object sender, RoutedEventArgs e)
        {
            if (MainTabControl.SelectedIndex == 0)
            {
                MyRecipesCategoryFilter("Pieczywo");
            }
            else
            {
                KooksRecipesCategoryFilter("Pieczywo");
            }
        }

        private void Others_Click(object sender, RoutedEventArgs e)
        {
            if (MainTabControl.SelectedIndex == 0)
            {
                MyRecipesCategoryFilter("Inne");
            }
            else
            {
                KooksRecipesCategoryFilter("Inne");
            }
        }
        private void SortingMyRecipes()
        {
            using (KooksDataBaseEntities context = new KooksDataBaseEntities())
            {
                int row = 0;

                MyRecipesGrid.Children.Clear();

                for (int i = 0; i < myRecipesArray.Length; i++)
                {
                    MyRecipesGrid.RowDefinitions.Add(new RowDefinition());
                    for (int column = 0; column < 4; column++)
                    {
                        ColumnDefinition columnDefinition = new ColumnDefinition();
                        columnDefinition.Width = new GridLength(197.8);
                        MyRecipesGrid.ColumnDefinitions.Add(columnDefinition);

                        MyRecipeControl myRecipeControl = new MyRecipeControl();

                        myRecipeControl.NameLabel.Content = myRecipesArray[i].Name;
                        myRecipeControl.RecipeImage.Source = ConvertByteArrayToBitmapImage(myRecipesArray[i].Image);
                        myRecipeControl.HoursLabel.Content = myRecipesArray[i].Time.Hours.ToString();
                        myRecipeControl.MinutesLabel.Content = myRecipesArray[i].Time.Minutes.ToString();
                        myRecipeControl.SecondsLabel.Content = myRecipesArray[i].Time.Seconds.ToString();
                        myRecipeControl.Rating = myRecipesArray[i].Level;
                        myRecipeControl.MouseDoubleClick += MyRecipeControl_MouseDoubleClick;

                        Grid.SetColumn(myRecipeControl, column);
                        Grid.SetRow(myRecipeControl, row);
                        MyRecipesGrid.Children.Add(myRecipeControl);

                        if (myRecipesArray[i] == myRecipesArray.Last())
                        {
                            break;
                        }
                        if (column < 3)
                        {
                            i++;
                        }
                    }
                    row++;
                }
            }
        }

        // mozna myrecipecontrol nazwac neutralnie - recipeControl
        private void SortingKooksRecipes()
        {
            using (KooksDataBaseEntities context = new KooksDataBaseEntities())
            {
                int row = 0;

                KooksRecipesGrid.Children.Clear();

                for (int i = 0; i < kooksRecipesArray.Length; i++)
                {
                    KooksRecipesGrid.RowDefinitions.Add(new RowDefinition());
                    for (int column = 0; column < 4; column++)
                    {
                        ColumnDefinition columnDefinition = new ColumnDefinition();
                        columnDefinition.Width = new GridLength(197.8);
                        KooksRecipesGrid.ColumnDefinitions.Add(columnDefinition);

                        MyRecipeControl myRecipeControl = new MyRecipeControl();

                        myRecipeControl.NameLabel.Content = kooksRecipesArray[i].Name;
                        myRecipeControl.RecipeImage.Source = ConvertByteArrayToBitmapImage(kooksRecipesArray[i].Image);
                        myRecipeControl.HoursLabel.Content = kooksRecipesArray[i].Time.Hours.ToString();
                        myRecipeControl.MinutesLabel.Content = kooksRecipesArray[i].Time.Minutes.ToString();
                        myRecipeControl.SecondsLabel.Content = kooksRecipesArray[i].Time.Seconds.ToString();
                        myRecipeControl.Rating = kooksRecipesArray[i].Level;
                        myRecipeControl.MouseDoubleClick += MyRecipeControl_MouseDoubleClick;

                        Grid.SetColumn(myRecipeControl, column);
                        Grid.SetRow(myRecipeControl, row);
                        KooksRecipesGrid.Children.Add(myRecipeControl);

                        if (kooksRecipesArray[i] == kooksRecipesArray.Last())
                        {
                            break;
                        }
                        if (column < 3)
                        {
                            i++;
                        }
                    }
                    row++;
                }
            }
        }

        // to polaczenie sortowania i filtrowania chyba lepiej bedzie zrobic bindowaniem jakims
        private void AscendingNameSort_Selected(object sender, RoutedEventArgs e)
        {
            AscendingNameSort();

        }

        private void AscendingNameSort()
        {
            if (MainTabControl.SelectedIndex == 0)
            {
                myRecipesArray = (from item in myRecipesArray orderby item.Name select item).ToArray();
                SortingMyRecipes(); // powinno byc nazwane FillingMyRecipes czy jakos
            }
            else
            {
                kooksRecipesArray = (from item in kooksRecipesArray orderby item.Name select item).ToArray();
                SortingKooksRecipes();
            }
        }

        private void DescendingNameSort_Selected(object sender, RoutedEventArgs e)
        {
            DescendingNameSort();
        }

        private void DescendingNameSort()
        {
            using (KooksDataBaseEntities context = new KooksDataBaseEntities()) // to sie tu chyba nie przyda
            {
                if (MainTabControl.SelectedIndex == 0)
                {
                    myRecipesArray = (from item in myRecipesArray orderby item.Name descending select item).ToArray();

                    SortingMyRecipes(); // to sie powinno nazywac inaczej, fill czy cos
                }
                else
                {
                    kooksRecipesArray = (from item in kooksRecipesArray orderby item.Name descending select item).ToArray();
                    SortingKooksRecipes();
                }
            }
        }

        private void AscendingDateSort_Selected(object sender, RoutedEventArgs e)
        {
            AscendingCreationDateSort();
        }

        private void DescendingDateSort_Selected(object sender, RoutedEventArgs e)
        {
            DescendingCreationDateSort();
        }

        private void AscendingLevelSort_Selected(object sender, RoutedEventArgs e)
        {
            AscendingLevelSort();
        }
        private void AscendingLevelSort()
        {
            using (KooksDataBaseEntities context = new KooksDataBaseEntities())
            {
                if (MainTabControl.SelectedIndex == 0)
                {
                    myRecipesArray = (from item in myRecipesArray orderby item.Level select item).ToArray();
                    SortingMyRecipes();
                }
                else
                {
                    kooksRecipesArray = (from item in kooksRecipesArray orderby item.Level select item).ToArray();
                    SortingKooksRecipes();
                }
            }
        }

        private void DescendingLevelSort_Selected(object sender, RoutedEventArgs e)
        {
            DescendingLevelSort();
        }

        private void DescendingLevelSort()
        {
            using (KooksDataBaseEntities context = new KooksDataBaseEntities())
            {
                if (MainTabControl.SelectedIndex == 0)
                {
                    myRecipesArray = (from item in myRecipesArray orderby item.Level descending select item).ToArray();
                    SortingMyRecipes();
                }
                else
                {
                    kooksRecipesArray = (from item in kooksRecipesArray orderby item.Level descending select item).ToArray();
                    SortingKooksRecipes();
                }
            }
        }

        private void AscendingTimeSort_Selected(object sender, RoutedEventArgs e)
        {
            AscendingTimeSort();
        }

        private void AscendingTimeSort()
        {
            using (KooksDataBaseEntities context = new KooksDataBaseEntities())
            {
                if (MainTabControl.SelectedIndex == 0)
                {
                    myRecipesArray = (from item in myRecipesArray orderby item.Time select item).ToArray();
                    SortingMyRecipes();
                }
                else
                {
                    kooksRecipesArray = (from item in kooksRecipesArray orderby item.Time select item).ToArray();
                    SortingKooksRecipes();
                }

            }
        }

        private void DescendingTimeSort_Selected(object sender, RoutedEventArgs e)
        {
            DescendingTimeSort();
        }

        private void DescendingTimeSort()
        {
            using (KooksDataBaseEntities context = new KooksDataBaseEntities())
            {
                if (MainTabControl.SelectedIndex == 0)
                {
                    myRecipesArray = (from item in myRecipesArray orderby item.Time descending select item).ToArray();
                    SortingMyRecipes();
                }
                else
                {
                    kooksRecipesArray = (from item in kooksRecipesArray orderby item.Time descending select item).ToArray();
                    SortingKooksRecipes();
                }
            }
        }

        private void AscendingCreationDateSort()
        {
            using (KooksDataBaseEntities context = new KooksDataBaseEntities())
            {
                myRecipesArray = (from item in myRecipesArray orderby item.Creation_Date select item).ToArray();
                SortingMyRecipes();
            }
        }

        private void DescendingCreationDateSort()
        {
            using (KooksDataBaseEntities context = new KooksDataBaseEntities())
            {
                myRecipesArray = (from item in myRecipesArray orderby item.Creation_Date descending select item).ToArray();
                SortingMyRecipes();
            }
        }

        private void MyRecipesSearchComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {

                MyRecipeControl myRecipeControl = new MyRecipeControl();

                using (KooksDataBaseEntities context = new KooksDataBaseEntities())
                {
                    foreach (var item in context.MyRecipes)
                    {
                        if (item.Name == MyRecipesSearchComboBox.Text)
                        {
                            myRecipeControl.NameLabel.Content = item.Name;
                            myRecipeControl.RecipeImage.Source = ConvertByteArrayToBitmapImage(item.Image);
                            myRecipeControl.HoursLabel.Content = item.Time.Hours.ToString();
                            myRecipeControl.MinutesLabel.Content = item.Time.Minutes.ToString();
                            myRecipeControl.SecondsLabel.Content = item.Time.Seconds.ToString();
                            myRecipeControl.Rating = item.Level;
                            myRecipeControl.MouseDoubleClick += MyRecipeControl_MouseDoubleClick;

                            MyRecipesGrid.Children.Clear();
                            MyRecipesGrid.Children.Add(myRecipeControl);
                            foreach (var child in MyRecipesRadioButtonsGrid.Children)
                            {
                                if (child is RadioButton)
                                {
                                    RadioButton radioButton = (RadioButton)child;
                                    radioButton.IsChecked = false;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void KooksRecipesSearchComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {

                MyRecipeControl myRecipeControl = new MyRecipeControl();

                using (KooksDataBaseEntities context = new KooksDataBaseEntities())
                {
                    foreach (var item in context.KooksRecipes)
                    {
                        if (item.Name == KooksRecipesSearchComboBox.Text)
                        {
                            myRecipeControl.NameLabel.Content = item.Name;
                            myRecipeControl.RecipeImage.Source = ConvertByteArrayToBitmapImage(item.Image);
                            myRecipeControl.HoursLabel.Content = item.Time.Hours.ToString();
                            myRecipeControl.MinutesLabel.Content = item.Time.Minutes.ToString();
                            myRecipeControl.SecondsLabel.Content = item.Time.Seconds.ToString();
                            myRecipeControl.Rating = item.Level;
                            myRecipeControl.MouseDoubleClick += MyRecipeControl_MouseDoubleClick;

                            KooksRecipesGrid.Children.Clear();
                            KooksRecipesGrid.Children.Add(myRecipeControl);
                            foreach (var child in KooksRecipesRadioButtonsGrid.Children)
                            {
                                if (child is RadioButton)
                                {
                                    RadioButton radioButton = (RadioButton)child;
                                    radioButton.IsChecked = false;
                                }
                            }
                        }
                    }
                }

            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainTabControl.SelectedIndex == 0)
            {
                var result = MessageBox.Show("Czy na pewno chcesz usunąć zaznaczone przepisy?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {

                    using (KooksDataBaseEntities context = new KooksDataBaseEntities())
                    {
                        if (MainTabControl.SelectedIndex == 0)
                        {
                            foreach (var child in MyRecipesGrid.Children)
                            {
                                MyRecipeControl myRecipeControl = (MyRecipeControl)child;

                                if (myRecipeControl.Clicked == true)
                                {
                                    var myRecipe = (from item in context.MyRecipes where item.Name == (string)myRecipeControl.NameLabel.Content select item).ToArray();

                                    MyRecipe myRecipeToDelete = context.MyRecipes.Find(myRecipe[0].RecipeID);
                                    context.MyRecipes.Remove(myRecipeToDelete);
                                }
                            }
                            context.SaveChanges();
                            LoadMyRecipes();
                        }
                    }
                }
            }
        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                if (MainTabControl.SelectedIndex == 1)
                {
                    DeleteButton.Visibility = Visibility.Collapsed;
                }
                else
                {
                    DeleteButton.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
