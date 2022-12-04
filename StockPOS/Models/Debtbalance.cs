using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockPOS.Models
{
    [Table("debtbalance")]
    [Index("CustomerId", Name = "DebtToCustomer_idx")]
    [Index("SaleId", Name = "DebtToSale_idx")]
    [Index("UserId", Name = "DebtToUserID_idx")]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public partial class Debtbalance
    {
        [Key]
        [Column("DebtBalanceID")]
        public int DebtBalanceId { get; set; }
        [Column("CustomerID")]
        public int? CustomerId { get; set; }
        public double? DebtAmount { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column("UserID")]
        public int? UserId { get; set; }
        [Column("SaleID")]
        public int? SaleId { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("Debtbalances")]
        public virtual Customer? Customer { get; set; }
        [ForeignKey("SaleId")]
        [InverseProperty("Debtbalances")]
        public virtual Sale? Sale { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("Debtbalances")]
        public virtual User? User { get; set; }
    }
}
