using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

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
        public virtual DbSet<Provider> Providers { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<SaleDetail> SaleProducts { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<ExportWarehouse> ExportWarehouses { get; set; }
        public virtual DbSet<ImportWarehouse> ImportWarehouses { get; set; }
        public virtual DbSet<ImportWarehouseDetail> ImportWarehouseDetails { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificationType> NotificationTypes { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<RatingDetail> RatingDetails { get; set; }
        public virtual DbSet<Recommendation> Recommendations { get; set; }
        public virtual DbSet<RecommendationDetail> RecommendationDetails { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<ProductDetail> ProductDetails { get; set; }
        public virtual DbSet<Banner> Banners { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<Ward> Wards { get; set; }

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

            // Entity Bill
            modelBuilder.Entity<Bill>(e =>
            {
                e.HasKey(x => x.BillId);

                e.HasIndex(x => x.BillId);

                e.HasOne(x => x.Order)
                .WithOne(x => x.Bill)
                .HasForeignKey<Bill>(x => x.OrderId);

                e.Property(x => x.Total).HasPrecision(18, 2);
                e.Property(x => x.PurchaseTotal).HasPrecision(18, 2);
                e.Property(x => x.SaleTotal).HasPrecision(18, 2);
            });

            // Entity ExportWarehouse
            modelBuilder.Entity<ExportWarehouse>(e =>
            {
                e.HasKey(x => x.ExportWarehouseId);

                e.HasIndex(x => x.ExportWarehouseId);
                e.HasIndex(x => x.OrderId);

                e.HasOne(x => x.Order)
                .WithOne(x => x.ExportWarehouse)
                .HasForeignKey<ExportWarehouse>(x => x.OrderId);

                e.Property(x => x.Total).HasPrecision(18, 2);
            });

            // Entity Order
            modelBuilder.Entity<Order>(e =>
            {
                e.HasKey(x => x.OrderId);

                e.HasIndex(x => x.OrderId);

                e.HasOne(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.UserId);

                e.Property(x => x.Total).HasPrecision(18, 2);
            });


            // Entity User
            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasIndex(x => x.Id);

                e.HasOne(x => x.Ward)
                .WithOne(x => x.User)
                .HasForeignKey<User>(x => x.WardCode);
            });

            // Entity OrderDetil
            modelBuilder.Entity<OrderDetail>(e =>
            {
                e.HasKey(x => new { x.ProductDetailId , x.OrderId});

                e.HasIndex(x => x.ProductDetailId);
                e.HasIndex(x => x.OrderId);

                e.HasOne(x => x.Order)
                .WithMany(x => x.OrderDetails)
                .HasForeignKey(x => x.OrderId);

                e.HasOne(x => x.ProductDetail)
                .WithMany(x => x.OrderDetails)
                .HasForeignKey(x => x.ProductDetailId);

                e.Property(x => x.SalePrice).HasPrecision(18, 2);
                e.Property(x => x.Price).HasPrecision(18, 2);
            });

            //Entity Product
            modelBuilder.Entity<Product>(e =>
            {
                e.HasKey(x => x.ProductId);
                e.HasIndex(x => x.ProductId);
                e.HasIndex(x => x.CategoryId);

                e.HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId);

                e.Property(x => x.Price).HasPrecision(18, 2);
            });

            // Entity Rating
            modelBuilder.Entity<Rating>(e =>
            {
                e.HasKey(x => x.RatingId);
                e.HasIndex(x => x.RatingId);
            });

            // Entity RatingDetail
            modelBuilder.Entity<RatingDetail>(e =>
            {
                e.HasKey(x => new { x.RatingId, x.UserId, x.ProductId });

                e.HasIndex(x => x.RatingId);
                e.HasIndex(x => x.UserId);
                e.HasIndex(x => x.ProductId);

                e.HasOne(x => x.Rating)
                .WithMany(x => x.RatingDetails)
                .HasForeignKey(x => x.RatingId);

                e.HasOne(x => x.User)
                .WithMany(x => x.RatingDetails)
                .HasForeignKey(x => x.UserId);

                e.HasOne(x => x.Product)
                .WithMany(x => x.RatingDetails)
                .HasForeignKey(x => x.ProductId);
            });

            //Entity NotificationType
            modelBuilder.Entity<NotificationType>(e =>
            {
                e.HasKey(x => x.NotificationTypeId);
                e.HasIndex(x => x.NotificationTypeId);

            });

            //Entity Notification
            modelBuilder.Entity<Notification>(e =>
            {
                e.HasKey(x => new { x.NotificationTypeId, x.UserId });

                e.HasIndex(x => x.NotificationTypeId);
                e.HasIndex(x => x.UserId);

                e.HasOne(x => x.NotificationType)
                .WithMany(x => x.Notifications)
                .HasForeignKey(x => x.NotificationTypeId);

                e.HasOne(x => x.User)
                .WithMany(x => x.Notifications)
                .HasForeignKey(x => x.UserId);
            });

            //Entity Category
            modelBuilder.Entity<Category>(e =>
            {
                e.HasKey(x => x.CategoryId);
                e.HasIndex(x => x.CategoryId);
            });

            //Entity Sale
            modelBuilder.Entity<Sale>(e =>
            {
                e.HasKey(x => x.SaleId);
                e.HasIndex(x => x.SaleId);

                e.Property(x => x.Discount).HasPrecision(18, 2);
            });

            //Entity SaleDetail
            modelBuilder.Entity<SaleDetail>(e =>
            {
                e.HasKey(x => new { x.SaleId, x.ProductId});

                e.HasIndex(x => x.SaleId);
                e.HasIndex(x => x.ProductId);

                e.HasOne(x => x.Sale)
                .WithMany(x => x.SaleDetails)
                .HasForeignKey(x => x.SaleId);

                e.HasOne(x => x.Product)
                .WithMany(x => x.SaleDetails)
                .HasForeignKey(x => x.ProductId);
            });

            //Entity Provider
            modelBuilder.Entity<Provider>(e =>
            {
                e.HasKey(x => x.ProviderId);
                e.HasIndex(x => x.ProviderId);
            });

            //Entity ImportWarehouse
            modelBuilder.Entity<ImportWarehouse>(e =>
            {
                e.HasKey(x => x.ImportWarehouseId);
                e.HasIndex(x => x.ImportWarehouseId);

                e.Property(x => x.Total).HasPrecision(18, 2);

                e.HasOne(x => x.ReceiptRequest)
                .WithOne(x => x.ImportWarehouse)
                .HasForeignKey<ImportWarehouse>(x => x.ReceiptRequestId);
            });

            //Entity ImportWarehouseDetail
            modelBuilder.Entity<ImportWarehouseDetail>(e =>
            {
                e.HasKey(x => new { x.ProductDetailId, x.ImportWarehouseDetailId, x.ProviderId });

                e.HasIndex(x => x.ProductDetailId);
                e.HasIndex(x => x.ProviderId);
                e.HasIndex(x => x.ImportWarehouseDetailId);

                e.HasOne(d => d.ProductDetail)
                .WithMany(p => p.ImportWarehouseDetails)
                .HasForeignKey(d => d.ProductDetailId);

                e.HasOne(d => d.Provider)
                .WithMany(p => p.ImportWarehouseDetails)
                .HasForeignKey(d => d.ProviderId);

                e.HasOne(d => d.ImportWarehouse)
                .WithMany(p => p.ImportWarehouseDetails)
                .HasForeignKey(d => d.ImportWarehouseDetailId);

                e.Property(x => x.ImportPrice).HasPrecision(18, 2);
            });

            //Entity Recommendation
            modelBuilder.Entity<Recommendation>(e =>
            {
                e.HasKey(x => x.RecommendtionId);
                e.HasIndex(x => x.RecommendtionId);
            });

            //Entity RecommendationDetail
            modelBuilder.Entity<RecommendationDetail>(e =>
            {
                e.HasKey(x => new { x.ProductId , x.RecommendationId});

                e.HasIndex(x => x.ProductId);
                e.HasIndex(x => x.RecommendationId);

                e.HasOne(x => x.Product)
                .WithMany(x => x.RecommendationDetails)
                .HasForeignKey(x => x.ProductId);

                e.HasOne(x => x.Recommendation)
                .WithMany(x => x.RecommendationDetails)
                .HasForeignKey(x => x.RecommendationId);
            });

            //Entity ProductImage
            modelBuilder.Entity<ProductImage>(e =>
            {
                e.HasKey(x => x.ProductImageId);
                e.HasIndex(x => x.ProductImageId);
                e.HasIndex(x => x.ProductId);

                e.HasOne(x => x.Product)
                .WithMany(x => x.ProductImages)
                .HasForeignKey(x => x.ProductId);
            });

            // Entity ProductDetail
            modelBuilder.Entity<ProductDetail>(e =>
            {
                e.HasKey(x => x.ProductDetailId);

                e.HasIndex(x => x.ProductId);
                e.HasIndex(x => x.ProductDetailId);

                e.HasOne(x => x.Product)
                .WithMany(x => x.ProductDetails)
                .HasForeignKey(x => x.ProductId);
            });

            // Entity Banner
            modelBuilder.Entity<Banner>(e =>
            {
                e.HasKey(x => x.BannerId);
                e.HasIndex(x => x.BannerId);
            });

            // Entity CartItem
            modelBuilder.Entity<CartItem>(e =>
            {
                e.HasKey(x => new { x.ProductDetailId, x.UserId });
                e.HasIndex(x => x.UserId);
                e.HasIndex(x => x.ProductDetailId);

                e.HasOne(x => x.ProductDetail)
                .WithMany(x => x.CartItems)
                .HasForeignKey(x => x.ProductDetailId);

                e.HasOne(x => x.User)
                .WithMany(x => x.CartItems)
                .HasForeignKey(x => x.UserId);

                e.Property(x => x.Price).HasPrecision(18, 2);
            });

            // Entity Province
            modelBuilder.Entity<Province>(e =>
            {
                e.HasKey(x => x.ProvinceID);
                e.HasIndex(x => x.ProvinceID);
                e.HasIndex(x => x.Code);
            });

            // Entity District
            modelBuilder.Entity<District>(e =>
            {
                e.HasKey(x => x.DistrictID);
                e.HasIndex(x => x.DistrictID);
                e.HasIndex(x => x.ProvinceID);

                e.HasOne(x => x.Province)
                .WithMany(x => x.Districts)
                .HasForeignKey(x => x.ProvinceID);
            });

            // Entity Ward
            modelBuilder.Entity<Ward>(e =>
            {
                e.HasKey(x => x.WardCode);
                e.HasIndex(x => x.WardCode);
                e.HasIndex(x => x.DistrictID);

                e.HasOne(x => x.District)
                .WithMany(x => x.Wards)
                .HasForeignKey(x => x.DistrictID);
            });

            // Entity Receipt Request
            modelBuilder.Entity<ReceiptRequest>(e =>
            {
                e.HasKey(x => x.ReceiptRequestId);
                e.HasIndex(x => x.ReceiptRequestId);
            });

            //Entity Receipt Request Detail
            modelBuilder.Entity<ReceiptRequestDetail>(e =>
            {
                e.HasKey(x => new { x.ProductDetailId, x.ReceiptRequestId });
                e.HasIndex(x => x.ReceiptRequestId);
                e.HasIndex(x => x.ProductDetailId);

                e.HasOne(x => x.ReceiptRequest)
                .WithMany(x => x.ReceiptRequestDetails)
                .HasForeignKey(x => x.ReceiptRequestId);

                e.HasOne(x => x.ProductDetail)
                .WithMany(x => x.ReceiptRequestDetails)
                .HasForeignKey(x => x.ProductDetailId);
            });
        }
    }
}
