using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockPOS.Models
{
    [Table("productsize")]
    [Index("ProductTypeId", Name = "ProuctTypeTable_idx")]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public partial class Productsize
    {
        [Key]
        [Column("ProductSizeID")]
        public int ProductSizeId { get; set; }
        [StringLength(45)]
        public string? ProductSizeName { get; set; }
        [Column("ProductTypeID")]
        [StringLength(15)]
        public string? ProductTypeId { get; set; }

        [ForeignKey("ProductTypeId")]
        [InverseProperty("Productsizes")]
        public virtual Producttype? ProductType { get; set; }
    }
}
