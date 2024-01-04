using System.Windows;
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
            var categories = await categoryService.GetAllCategoriesAsync();
            CategoryDataGrid.ItemsSource = categories;
        }

        private async void ReloadRecords_Click(object sender, RoutedEventArgs e)
        {
            var categories = await categoryService.GetAllCategoriesAsync();
            CategoryDataGrid.ItemsSource = categories;
        }

        private void AddRecord_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditRecord_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteRecords_Click(object sender, RoutedEventArgs e)
        {

        }

       
    }
}
