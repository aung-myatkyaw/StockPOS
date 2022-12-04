using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockPOS.Models
{
    [Table("customer")]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public partial class Customer
    {
        public Customer()
        {
            Debtbalances = new HashSet<Debtbalance>();
        }

        [Key]
        [Column("CustomerID")]
        public int CustomerId { get; set; }
        [StringLength(45)]
        public string? CustomerName { get; set; }
        [Column("VillageID")]
        public int? VillageId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }

        [InverseProperty("Customer")]
        public virtual ICollection<Debtbalance> Debtbalances { get; set; }
    }
}
