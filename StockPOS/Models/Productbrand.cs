using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockPOS.Models
{
    [Table("productbrand")]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public partial class Productbrand
    {
        public Productbrand()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        [Column("BrandID")]
        [StringLength(15)]
        public string BrandId { get; set; } = null!;
        [StringLength(45)]
        public string? BrandName { get; set; }

        [InverseProperty("Brand")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
