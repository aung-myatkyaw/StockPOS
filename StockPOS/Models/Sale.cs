using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockPOS.Models
{
    [Table("sale")]
    [Index("ProductId", Name = "SaleToProduct_idx")]
    [Index("UserId", Name = "SaleToUser_idx")]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public partial class Sale
    {
        public Sale()
        {
            Debtbalances = new HashSet<Debtbalance>();
        }

        [Key]
        [Column("SaleID")]
        public int SaleId { get; set; }
        [StringLength(30)]
        public string? VouncherNum { get; set; }
        [Column("UserID")]
        public int? UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? SaleDateTime { get; set; }
        [Column("ProductID")]
        [StringLength(15)]
        public string? ProductId { get; set; }
        public int? Quantity { get; set; }
        public double? TotalAmount { get; set; }
        public double? ProductPrice { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("Sales")]
        public virtual Product? Product { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("Sales")]
        public virtual User? User { get; set; }
        [InverseProperty("Sale")]
        public virtual ICollection<Debtbalance> Debtbalances { get; set; }
    }
}
