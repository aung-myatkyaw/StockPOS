using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockPOS.Models.ViewModels
{
    public class Product_View_Model
    {
        [StringLength(50)]
        public string ProductId { get; set; }

        [StringLength(30)]
        public string? Barcode { get; set; }

        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(50)]
        public string? ShortName { get; set; }

        [StringLength(50)]
        public string? CategoryId { get; set; }

        [StringLength(50)]
        public string? CategoryName { get; set; }

        [Required]
        public double? BoughtPrice { get; set; }

        [Required]
        public double? SellPriceRetail { get; set; }

        [Required]
        public double? SellPricewhole { get; set; }

        [StringLength(200)]
        public string? ImageUrl { get; set; }

    }
}

