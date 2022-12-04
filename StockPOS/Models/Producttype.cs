using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockPOS.Models
{
    [Table("producttype")]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public partial class Producttype
    {
        public Producttype()
        {
            Products = new HashSet<Product>();
            Productsizes = new HashSet<Productsize>();
        }

        [Key]
        [Column("ProductTypeID")]
        [StringLength(15)]
        public string ProductTypeId { get; set; } = null!;
        [StringLength(100)]
        public string? ProductTypeName { get; set; }

        [InverseProperty("ProductType")]
        public virtual ICollection<Product> Products { get; set; }
        [InverseProperty("ProductType")]
        public virtual ICollection<Productsize> Productsizes { get; set; }
    }
}
