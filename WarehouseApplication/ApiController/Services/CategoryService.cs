using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseApiClient;
using WarehouseApplication.ApiController;

namespace WarehouseApplication.ApiController.Services
{
    public class CategoryService
    {
        private readonly swaggerClient client;

        public CategoryService(ApiClient apiClient)
        {
            this.client = apiClient.ConnectToApi().Result; // Oczekujemy na połączenie przy inicjalizacji
        }

        public async Task<ICollection<Category>> GetAllCategories()
        {            
            var result = await client.GetCategoriesAsync();
            return result;
        }

        public async Task<Category> GetSingleCategory(int id)
        {           
            var result = await client.GetCategoryAsync(id);
            return result;
        }
        public async Task DeleteCategory(int id)
        {
            await client.DeleteCategoryAsync(id);
        }
        public async Task UpdateCategory(int id, Category body)
        {
            await client.PutCategoryAsync(id, body);
        }
        public async Task<Category> PostCategory(Category body)
        {
            var result = await client.PostCategoryAsync(body);
            return result;
        }
        public async Task<ICollection<Product>> GetProductsByCategoryId(int categoryId)
        {
            var result = await client.GetProductsByCategoryAsync(categoryId);
            return result;
        }
    }
}
