using System.Windows;
using System.Windows.Controls;
using WarehouseApiClient;
using WarehouseApplication.ApiController.Services;

namespace WarehouseApplication
{
    /// <summary>
    /// Logika interakcji dla klasy CategoryDataWindow.xaml
    /// </summary>
    public partial class CategoryDataWindow : Window
    {
        
        private readonly CategoryService categoryService;
        public CategoryDataWindow(CategoryService categoryService)
        {
            InitializeComponent();

            this.categoryService = categoryService;



            Loaded += CategoryDataWindow_Loaded;
        }
        private async void CategoryDataWindow_Loaded(object sender,RoutedEventArgs e)
        {
            try
            {
                var categories = await categoryService.GetAllCategories();
                CategoryDataGrid.ItemsSource = categories;

            }
            catch (Exception ex){
                CategoryDataGrid.ItemsSource = null;                
                MessageBox.Show($"Wystąpił błąd: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ReloadCategories()
        {
            var categories = await categoryService.GetAllCategories();
            CategoryDataGrid.ItemsSource = categories;            
        }        

        private void AddRecord_Click(object sender, RoutedEventArgs e)
        {
            CategoryEditPanel.Visibility = Visibility.Hidden;
            CategoryAddPanel.Visibility = Visibility.Visible;
        }

        private void EditRecord_Click(object sender, RoutedEventArgs e)
        {
            Category selectedCategory = (Category)CategoryDataGrid.SelectedItem;
            if (CategoryDataGrid.SelectedItem != null)
            {
                CategoryAddPanel.Visibility = Visibility.Hidden;
                CategoryEditPanel.Visibility = Visibility.Visible;
                CategoryEditedName.Text = selectedCategory.Name;
                CategoryEditedDescription.Text = selectedCategory.Description;
            } else
            {
                MessageBox.Show("Choose a category to edit!");
            }
        }
        private async void SubmitEditCategory_Click(object sender, RoutedEventArgs e)
        {
            Category selectedCategory = (Category)CategoryDataGrid.SelectedItem;
            if (selectedCategory != null)
            {                
                var catId = selectedCategory.Id;
                    var body = new Category
                    {
                        Id = catId,
                        Name = CategoryEditedName.Text,
                        Description = CategoryEditedDescription.Text
                    };

                    await categoryService.UpdateCategory(catId, body);
                    await ReloadCategories();                
            }
            else { MessageBox.Show("Choose a category first!"); }
            }
        private async void DeleteRecord_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryDataGrid.SelectedItem != null)
            {
                Category selectedCategory = (Category)CategoryDataGrid.SelectedItem;
                await categoryService.DeleteCategory(selectedCategory.Id);
                await ReloadCategories();
            }
        }


        private async void SubmitNewCategory_Click(object sender, RoutedEventArgs e)
        {
            var newCategoryName = CategoryName.Text;
            var newCategoryDescription = CategoryDescription.Text;
            var body = new Category
            {
                Name = newCategoryName,
                Description = newCategoryDescription
            };

            try
            {
                // Twoje wywołanie API
                var result = await categoryService.PostCategory(body);
                await ReloadCategories();
            }
            catch (WarehouseApiClient.ApiException ex)
            {
                if (ex.StatusCode == 201)
                {
                    Console.WriteLine(ex.Message);
                }
                else
                {
                    // Obsługa innych błędów
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

    }
}
