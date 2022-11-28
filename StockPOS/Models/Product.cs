using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace StockPOS.Models
{
    [Table("product")]
    public partial class Product
    {
        [Key]
        [StringLength(30)]
        public string Barcode { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [Column("CategoryID", TypeName = "int(11)")]
        public int? CategoryId { get; set; }
        public double? BoughtPrice { get; set; }
        public double? SellPriceRetail { get; set; }
        public double? SellPricewhole { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateAdded { get; set; }
        [Column(TypeName = "int(11)")]
        public int? AlertQuantity { get; set; }
    }
}
