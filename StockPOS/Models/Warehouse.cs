using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockPOS.Models
{
    [Table("warehouse")]
    [Index("ProductId", Name = "WareHouseInToProduct_idx")]
    [Index("UserId", Name = "WareHouseInToUser_idx")]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public partial class Warehouse
    {
        [Key]
        [Column("WareHouseInTranID")]
        public int WareHouseInTranId { get; set; }
        [Column("ProductID")]
        [StringLength(15)]
        public string? ProductId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        public double? StockCount { get; set; }
        [StringLength(45)]
        public string? GoodSize { get; set; }
        [Column("UserID")]
        public int? UserId { get; set; }
        [StringLength(45)]
        public string? OtherColumn1 { get; set; }
        [StringLength(45)]
        public string? OtherColumn2 { get; set; }
        [StringLength(45)]
        public string? OtherColoum3 { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("Warehouses")]
        public virtual Product? Product { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("Warehouses")]
        public virtual User? User { get; set; }
    }
}
