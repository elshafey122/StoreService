using Microsoft.EntityFrameworkCore;
using Store.Core.Entities;
using Store.Core.Entities.Order_Aggregate;
using System.Reflection;


namespace Store.Repository.Data
{
    public class StoreContext:DbContext
    {

        public StoreContext(DbContextOptions<StoreContext>options)
            :base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }


        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }

    }
}
