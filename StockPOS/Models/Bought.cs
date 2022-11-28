using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace StockPOS.Models
{
    [Table("bought")]
    public partial class Bought
    {
        [Key]
        [Column("ID", TypeName = "int(11)")]
        public int Id { get; set; }
        [StringLength(30)]
        public string Barcode { get; set; }
        [Column(TypeName = "int(11)")]
        public int? StockAmount { get; set; }
        [Column("SupplierID", TypeName = "int(11)")]
        public int? SupplierId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? BoughtDateTime { get; set; }
        public double? TotalAmount { get; set; }
    }
}
