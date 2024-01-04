using Microsoft.EntityFrameworkCore;

namespace WarehouseApi.Entities
{
    public class WarehouseApiContext : DbContext
    {
        public WarehouseApiContext(DbContextOptions<WarehouseApiContext> options) : base(options)
        {
            
        }
        public DbSet<Category> Categories{ get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(eb =>
            {
                eb.Property(x => x.ProductName).IsRequired();
                eb.Property(x=>x.ProductDescription).IsRequired();
                eb.Property(x => x.Price).HasColumnType("decimal(5,2)");
            });
            modelBuilder.Entity<Category>(eb =>
            {
                eb.Property(eb => eb.Name).IsRequired();
                eb.Property(eb => eb.Description).IsRequired();
                eb.HasMany(x => x.Products).WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId);
                
            });

            

        }
    }
}
