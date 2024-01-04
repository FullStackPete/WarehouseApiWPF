using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WarehouseApiClient;

namespace WarehouseApplication.ApiController
{
    public class ApiClient
    {
        private readonly HttpClient httpClient;

        public ApiClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<swaggerClient> ConnectToApi()
        {
            return new swaggerClient("http://localhost:5047/", httpClient);
        }
        public async Task<swaggerClient> CheckForApiStatus()
        {
            var swaggerClient = new swaggerClient("http://localhost:5047/", httpClient);

            // Dodaj logikę sprawdzającą dostępność API
            try
            {
                // Wykonaj małe zapytanie, aby sprawdzić, czy API jest dostępne
                var categories = await swaggerClient.GetCategoriesAsync();
                return swaggerClient;
            }
            catch
            {
                // Jeżeli zapytanie zakończy się błędem, oznacza to brak dostępności API
                return null;
            }
        }
    }
}
