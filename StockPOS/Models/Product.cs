using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockPOS.Models
{
    [Table("product")]
    [Index("Barcode", Name = "Barcode_UNIQUE", IsUnique = true)]
    [Index("CategoryId", Name = "CategoryTable_idx")]
    [Index("BrandId", Name = "ProductBrand_idx")]
    [Index("ProductTypeId", Name = "ProductType_idx")]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public partial class Product
    {
        public Product()
        {
            Changedpricelogs = new HashSet<Changedpricelog>();
            Sales = new HashSet<Sale>();
            Searchedcounts = new HashSet<Searchedcount>();
            Warehouses = new HashSet<Warehouse>();
        }

        [Key]
        [Column("ProductID")]
        [StringLength(15)]
        public string ProductId { get; set; } = null!;
        [StringLength(30)]
        public string? Barcode { get; set; }
        [StringLength(100)]
        public string? Name { get; set; }
        [StringLength(10)]
        public string? ShortName { get; set; }
        [Column("CategoryID")]
        [StringLength(15)]
        public string? CategoryId { get; set; }
        [Column("ProductTypeID")]
        [StringLength(15)]
        public string? ProductTypeId { get; set; }
        [Column("BrandID")]
        [StringLength(15)]
        public string? BrandId { get; set; }
        public double? BoughtPrice { get; set; }
        public double? SellPriceRetail { get; set; }
        public double? SellPricewhole { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [StringLength(200)]
        public string? ImageUrl { get; set; }
        public int? AlertQuantity { get; set; }
        public sbyte? IsVisible { get; set; }

        [ForeignKey("BrandId")]
        [InverseProperty("Products")]
        public virtual Productbrand? Brand { get; set; }
        [ForeignKey("CategoryId")]
        [InverseProperty("Products")]
        public virtual Productcategory? Category { get; set; }
        [ForeignKey("ProductTypeId")]
        [InverseProperty("Products")]
        public virtual Producttype? ProductType { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<Changedpricelog> Changedpricelogs { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<Sale> Sales { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<Searchedcount> Searchedcounts { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<Warehouse> Warehouses { get; set; }
    }
}
