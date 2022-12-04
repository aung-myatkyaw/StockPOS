﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockPOS.Models
{
    [Table("productcategory")]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public partial class Productcategory
    {
        public Productcategory()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        [Column("CategoryID")]
        [StringLength(15)]
        public string CategoryId { get; set; } = null!;
        [StringLength(100)]
        public string? CategoryName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public sbyte? Visible { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
