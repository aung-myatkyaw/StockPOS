using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockPOS.Models.CreateModels
{
    public class ProductCreateModel
    {

        [StringLength(30)]
        public string? Barcode { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required]
        [StringLength(50)]
        public string? ShortName { get; set; }

        [Required]
        [Column("CategoryID")]
        [StringLength(50)]
        public string? CategoryId { get; set; }

        [Column("ProductTypeID")]
        [StringLength(50)]
        public string? ProductTypeId { get; set; }

        [Column("BrandID")]
        [StringLength(50)]
        public string? BrandId { get; set; }

        [Required]
        public double? BoughtPrice { get; set; }

        [Required]
        public double? SellPriceRetail { get; set; }

        [Required]
        public double? SellPricewhole { get; set; }

        [StringLength(200)]
        public string? ImageUrl { get; set; }

        public int? AlertQuantity { get; set; }

        public sbyte? IsVisible { get; set; }

    }
}

