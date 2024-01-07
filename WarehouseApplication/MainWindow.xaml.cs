using System.Net.Http;
using System.Windows;
using WarehouseApplication.ApiController;
using WarehouseApplication.ApiController.Services;
namespace WarehouseApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApiClient apiClient;
        private readonly CategoryService categoryService;
        private readonly ProductService productService;
        

        private string TextBlockContent // Zdefiniuj właściwość, aby przechowywać zawartość TextBlock
        {
            get { return ApiStatus.Text; }
            set { ApiStatus.Text = value; }
        }

        public MainWindow()
        {
            InitializeComponent();            

            var httpClient = new HttpClient();
            apiClient = new ApiClient(httpClient);
            categoryService = new CategoryService(apiClient);
            productService = new ProductService(apiClient);
            Loaded += MainWindow_Loaded;
        }


        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Metoda wykonywana przy starcie aplikacji            
            await CheckApi();
        }
        
        private async Task CheckApi()
        {
            TextBlockContent = "API status: ";
            try
            {
                var result = await apiClient.CheckForApiStatus();
                if (result != null)
                {
                    // Połączenie z API powiodło się
                    TextBlockContent +="Online!";
                }
                else
                {
                    // Result jest nullem, co oznacza problem z połączeniem
                    TextBlockContent += "Connection failure!";
                }
            }
            catch (Exception ex)
            {
                // Obsługa błędów
                TextBlockContent = $"Error: {ex.Message}";
            }
        }

        private async void ApiReconnect_Click(object sender, RoutedEventArgs e)
        {
            await CheckApi();
        }

        private void CategoryDataBtn(object sender, RoutedEventArgs e)
        {
            OpenCategoryDataWindow();
        }

        private void ProductDataBtn(object sender, RoutedEventArgs e)
        {
            OpenProductDataWindow();
        }
        private void OpenProductDataWindow()
        {
            ProductDataWindow productDataWindow = new ProductDataWindow(productService,categoryService);
            productDataWindow.Owner = this; // Ustaw właściciela nowego okna na główne okno
            productDataWindow.ShowDialog(); // Pokaż nowe okno jako dialog
        }
        private void OpenCategoryDataWindow()
        {
            CategoryDataWindow categoryDataWindow = new CategoryDataWindow(categoryService);
            categoryDataWindow.Owner = this; // Ustaw właściciela nowego okna na główne okno
            categoryDataWindow.ShowDialog(); // Pokaż nowe okno jako dialog
        }
    }
}