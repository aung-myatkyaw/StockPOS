using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace StockPOS.Models
{
    [Table("sale")]
    public partial class Sale
    {
        [Key]
        [Column("ID", TypeName = "int(11)")]
        public int Id { get; set; }
        [StringLength(30)]
        public string VouncherNum { get; set; }
        [Column("CashierID", TypeName = "int(11)")]
        public int? CashierId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? SaleDateTime { get; set; }
        [StringLength(30)]
        public string ProductBarcode { get; set; }
        [Column(TypeName = "int(11)")]
        public int? Quantity { get; set; }
        public double? TotalAmount { get; set; }
    }
}
