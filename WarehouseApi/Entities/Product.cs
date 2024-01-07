using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseApi.Entities
{
    public class Product
    {
        public int Id { get; set; }        
        public string ProductName { get; set; } = string.Empty;        
        public string ProductDescription { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public int? CategoryId {  get; set; }        
    }
}
