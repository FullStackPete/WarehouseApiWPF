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
using WarehouseApiClient;
using WarehouseApplication.ApiController.Services;

namespace WarehouseApplication
{
    /// <summary>
    /// Logika interakcji dla klasy ProductDataWindow.xaml
    /// </summary>
    public partial class ProductDataWindow : Window
    {
        private readonly ProductService productService;
        private readonly CategoryService categoryService;
        public ProductDataWindow(ProductService productService, CategoryService categoryService)
        {
            InitializeComponent();
            this.categoryService = categoryService;
            this.productService = productService;
            CategoryChoice.SelectionChanged += CategoryChoice_SelectionChanged;
            Loaded += ProductDataWindow_Loaded;
        }
        private async void ProductDataWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var products = await productService.GetAllProducts();
                
                ProductDataGrid.ItemsSource = products;
                var categories = (await categoryService.GetAllCategories()).ToList();
                var categoriesAdd = (await categoryService.GetAllCategories()).ToList();

               
                
                categories.Insert(0, new Category { Name = "Wszystkie kategorie" });                
                CategoryChoice.ItemsSource = categories;                
                CategoryChoice.SelectedIndex = 0;                
                CategoryChoice.DisplayMemberPath = "Name";


                CategoryChoiceAdd.ItemsSource = categoriesAdd;
                CategoryChoiceAdd.DisplayMemberPath = "Name";
                CategoryChoiceEdit.DisplayMemberPath= "Name";


            }
            catch (Exception ex)
            {
                ProductDataGrid.ItemsSource = null;
                MessageBox.Show($"Wystąpił błąd: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void CategoryChoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var defaultCategory = new Category
            { Name = "Wszystkie kategorie" };
            Category selectedCategory = (Category)CategoryChoice.SelectedItem;
            if(selectedCategory.Name == defaultCategory.Name)
            {
                var products = await productService.GetAllProducts();
                ProductDataGrid.ItemsSource = products;
                return;
            }
            if(selectedCategory == null) return;
            int categoryId = selectedCategory.Id;
            try
            {
                var products = await categoryService.GetProductsByCategoryId(categoryId);
                ProductDataGrid.ItemsSource = products;
            } catch (Exception ex) {
            ProductDataGrid.ItemsSource= null;
                MessageBox.Show($"Error occured: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddRecord_Click(object sender, RoutedEventArgs e)
        {
            ProductEditPanel.Visibility = Visibility.Hidden;
            ProductAddPanel.Visibility = Visibility.Visible;
        }

        private async void EditRecord_Click(object sender, RoutedEventArgs e)
        {
            Product selectedProduct = (Product)ProductDataGrid.SelectedItem;
            if (ProductDataGrid.SelectedItem != null)
            {
                ProductEditPanel.Visibility = Visibility.Visible;
                ProductAddPanel.Visibility = Visibility.Hidden;

                ProductEditedName.Text = selectedProduct.ProductName;
                ProductEditedDescription.Text = selectedProduct.ProductDescription; ;
                ProductEditedPrice.Text = selectedProduct.Price.ToString();

                var allCategories = await categoryService.GetAllCategories();
                CategoryChoiceEdit.ItemsSource = allCategories;

                var defaultCategory = allCategories.FirstOrDefault(x => x.Id == selectedProduct.CategoryId);
                CategoryChoiceEdit.SelectedItem = defaultCategory;
            }
            else
            {
                MessageBox.Show("Choose a product to edit!");
            }
        }
        private async void SubmitEditProduct_Click(object sender, RoutedEventArgs e)
        {
            Product selectedProduct = (Product)ProductDataGrid.SelectedItem;
            Category selectedCategory = (Category)CategoryChoiceEdit.SelectedItem;
            if (selectedProduct == null) {
                MessageBox.Show("Choose a product first!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }            
            var productId = selectedProduct.Id;
            
            var body = new Product
            {
                Id = productId,
                ProductName = ProductEditedName.Text,
                ProductDescription = ProductEditedDescription.Text,
                Price = double.Parse(ProductEditedPrice.Text),
                CategoryId =  selectedCategory.Id,
            };
            await productService.UpdateProduct(productId, body);
            MessageBox.Show($"Product {body.ProductName} edited!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            await ReloadProducts();
        }
        private async void SubmitNewProduct_Click(object sender, RoutedEventArgs e)
        {
            Category selectedCategory = (Category)CategoryChoiceAdd.SelectedItem;
            if (ProductPrice.Text == string.Empty || ProductName.Text == string.Empty || 
                ProductDescription.Text == string.Empty)
            {
                MessageBox.Show("Fields must not be empty!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            var newPrice = double.Parse(ProductPrice.Text);
            var body = new Product
            {
                ProductName = ProductName.Text,
                ProductDescription = ProductDescription.Text,
                Price = newPrice,
                CategoryId = selectedCategory.Id
            };
            try
            {
                await productService.PostProduct(body);
                
            } catch (ApiException ex)
            {
                if (ex.StatusCode == 201) { MessageBox.Show($"New product {body.ProductName} added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);  } // ignore positive response                
            }
            await ReloadProducts();
        }






        private async void DeleteRecord_Click(object sender, RoutedEventArgs e)
        {
            if (ProductDataGrid.SelectedItem != null)
            {
                Product selectedProduct =(Product)ProductDataGrid.SelectedItem;
                await productService.DeleteProduct(selectedProduct.Id);
                await ReloadProducts();
            }
        }
        private async Task ReloadProducts()
        {
            var products = await productService.GetAllProducts();
            ProductDataGrid.ItemsSource = products;
            CategoryChoice.SelectedIndex = 0;
        }

        
    }
}
