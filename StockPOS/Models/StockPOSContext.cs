using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using StockPOS.Settings;

#nullable disable

namespace StockPOS.Models
{
    public partial class StockPOSContext : DbContext
    {
        private IConfiguration _configuration;
        private string _connectionString;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IActionContextAccessor _actionContextAccessor;

        public StockPOSContext(DbContextOptions<StockPOSContext> options, IHttpContextAccessor httpContextAccessor, IActionContextAccessor actionContextAccessor, IConfiguration configuration)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _actionContextAccessor = actionContextAccessor;
            _configuration = configuration;
            var mysqlDbSettings = _configuration.GetSection(nameof(MysqlDbSettings)).Get<MysqlDbSettings>();
            _connectionString = mysqlDbSettings.ConnectionString;
        }

        public virtual DbSet<Bought> Boughts { get; set; }
        public virtual DbSet<Cashier> Cashiers { get; set; }
        public virtual DbSet<Eventlog> Eventlogs { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Productcategory> Productcategories { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(_connectionString, ServerVersion.Parse("8.0.31-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            modelBuilder.Entity<Eventlog>(entity =>
            {
                entity.Property(e => e.LogType).HasComment("Info = 1, Error = 2,Warning = 3, Insert = 4,Update = 5, Delete = 6");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Barcode)
                    .HasName("PRIMARY");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
