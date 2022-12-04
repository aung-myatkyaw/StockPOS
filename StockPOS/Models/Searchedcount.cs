using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockPOS.Models
{
    [Table("searchedcount")]
    [Index("ProductId", Name = "SearchToProduct_idx")]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public partial class Searchedcount
    {
        [Key]
        [Column("SearchID")]
        public int SearchId { get; set; }
        public int? Count { get; set; }
        [Column("ProductID")]
        [StringLength(15)]
        public string? ProductId { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("Searchedcounts")]
        public virtual Product? Product { get; set; }
    }
}
