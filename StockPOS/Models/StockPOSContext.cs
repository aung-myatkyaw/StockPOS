using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace StockPOS.Models
{
    public partial class StockPOSContext : DbContext
    {
        //private IConfiguration _configuration;
        //private string _connectionString;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IActionContextAccessor _actionContextAccessor;
        //private MySqlServerVersion serverVersion;

        public StockPOSContext(DbContextOptions<StockPOSContext> options, IHttpContextAccessor httpContextAccessor, IActionContextAccessor actionContextAccessor, IConfiguration configuration)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _actionContextAccessor = actionContextAccessor;
            //_configuration = configuration;
            //var mysqlDbSettings = _configuration.GetSection(nameof(MysqlDbSettings)).Get<MysqlDbSettings>();
            //_connectionString = mysqlDbSettings.ConnectionString;

            //serverVersion = new MySqlServerVersion(new Version(8, 0, 31));
        }

        public virtual DbSet<Changedpricelog> Changedpricelogs { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Debtbalance> Debtbalances { get; set; } = null!;
        public virtual DbSet<Eventlog> Eventlogs { get; set; } = null!;
        public virtual DbSet<Groupvillage> Groupvillages { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Productbrand> Productbrands { get; set; } = null!;
        public virtual DbSet<Productcategory> Productcategories { get; set; } = null!;
        public virtual DbSet<Productsize> Productsizes { get; set; } = null!;
        public virtual DbSet<Producttype> Producttypes { get; set; } = null!;
        public virtual DbSet<Sale> Sales { get; set; } = null!;
        public virtual DbSet<Searchedcount> Searchedcounts { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Usertype> Usertypes { get; set; } = null!;
        public virtual DbSet<Village> Villages { get; set; } = null!;
        public virtual DbSet<Warehouse> Warehouses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Changedpricelog>(entity =>
            {
                entity.HasKey(e => e.PriceLogId)
                    .HasName("PRIMARY");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Changedpricelogs)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("PriceLogToProduct");
            });

            modelBuilder.Entity<Debtbalance>(entity =>
            {
                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Debtbalances)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("DebtToCustomer");

                entity.HasOne(d => d.Sale)
                    .WithMany(p => p.Debtbalances)
                    .HasForeignKey(d => d.SaleId)
                    .HasConstraintName("DebtToSale");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Debtbalances)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("DebtToUserID");
            });

            modelBuilder.Entity<Eventlog>(entity =>
            {
                entity.Property(e => e.LogType).HasComment("Info = 1, Error = 2,Warning = 3, Insert = 4,Update = 5, Delete = 6");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("ProductBrand");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("CategoryTable");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductTypeId)
                    .HasConstraintName("ProductType");
            });

            modelBuilder.Entity<Productbrand>(entity =>
            {
                entity.HasKey(e => e.BrandId)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<Productcategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<Productsize>(entity =>
            {
                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.Productsizes)
                    .HasForeignKey(d => d.ProductTypeId)
                    .HasConstraintName("ProuctTypeTable");
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("SaleToProduct");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("SaleToUser");
            });

            modelBuilder.Entity<Searchedcount>(entity =>
            {
                entity.HasKey(e => e.SearchId)
                    .HasName("PRIMARY");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Searchedcounts)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("SearchToProduct");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasOne(d => d.UserType)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserToUserType");
            });

            modelBuilder.Entity<Village>(entity =>
            {
                entity.HasOne(d => d.GroupVillage)
                    .WithMany(p => p.Villages)
                    .HasForeignKey(d => d.GroupVillageId)
                    .HasConstraintName("VillageToGroupVillage");
            });

            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.HasKey(e => e.WareHouseInTranId)
                    .HasName("PRIMARY");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Warehouses)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("WareHouseInToProduct");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Warehouses)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("WareHouseInToUser");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
