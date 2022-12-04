using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockPOS.Models
{
    [Table("changedpricelog")]
    [Index("ProductId", Name = "PriceLogToProduct_idx")]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public partial class Changedpricelog
    {
        [Key]
        [Column("PriceLogID")]
        public int PriceLogId { get; set; }
        [Column("ProductID")]
        [StringLength(15)]
        public string? ProductId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ChangedDate { get; set; }
        public double? LatestBoughtPrice { get; set; }
        public double? LatestRetailPrice { get; set; }
        public double? LatestWholePrice { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("Changedpricelogs")]
        public virtual Product? Product { get; set; }
    }
}
