using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseApiClient;
using WarehouseApplication.ApiController;

namespace WarehouseApplication.ApiController.Services
{
    public class ProductService
    {
        private readonly swaggerClient client;
        public ProductService(ApiClient apiClient)
        {
            this.client = apiClient.ConnectToApi().Result; // Oczekujemy na połączenie przy inicjalizacji
        }

        public async Task<ICollection<Product>> GetAllProducts()
        {
            
            var result = await client.GetProductsAsync();            
            return result;
        }
        public async Task<Product> GetSingleProduct(int id)
        {
            
            var result = await client.GetProductAsync(id);
            return result;
        }
        public async Task<Product> PostProduct(Product body)
        {
            
            var result = await client.PostProductAsync(body);
            return result;
        }
        public async Task DeleteProduct(int id)
        {
            
            await client.DeleteProductAsync(id);

        }
        public async Task UpdateProduct(int id, Product body)
        {
            
            await client.PutProductAsync(id, body);
        }
    }
}
