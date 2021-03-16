using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Entities.Data
{
    public class ShopDbContext : IdentityDbContext<User>
    {
        public ShopDbContext()
        {

        }

        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductProvider> ProductProviders { get; set; }
        public virtual DbSet<Provider> Providers { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<SaleProduct> SaleProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            modelBuilder.Entity<OrderDetail>()
                    .HasKey(o => new { o.OrderId, o.ProductId });
            modelBuilder.Entity<ProductProvider>()
                    .HasKey(p => new { p.ProductId, p.ProviderId });
            modelBuilder.Entity<SaleProduct>()
                    .HasKey(s => new { s.SaleId, s.ProductId });
        }
    }
}
