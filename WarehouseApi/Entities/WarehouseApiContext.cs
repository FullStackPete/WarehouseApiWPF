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
            modelBuilder.Entity<Category>(eb =>
            {
                eb.Property(eb => eb.Name).IsRequired();
                eb.Property(eb => eb.Description).IsRequired();

                eb.HasMany(e => e.Products).WithOne()
                .HasForeignKey(e => e.CategoryId).IsRequired();
            });
            modelBuilder.Entity<Product>(eb =>
            {
                eb.Property(x => x.ProductName).IsRequired();
                eb.Property(x=>x.ProductDescription).IsRequired();
                eb.Property(x => x.Price).HasColumnType("decimal(5,2)");
            });
                     
        }
    }
}
