using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockPOS.Models
{
    [Table("user")]
    [Index("UserTypeId", Name = "UserToUserType_idx")]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public partial class User
    {
        public User()
        {
            Debtbalances = new HashSet<Debtbalance>();
            Sales = new HashSet<Sale>();
            Warehouses = new HashSet<Warehouse>();
        }

        [Key]
        [Column("UserID")]
        public int UserId { get; set; }
        [StringLength(50)]
        public string? FullName { get; set; }
        [StringLength(50)]
        public string UserName { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime? DateofBirth { get; set; }
        [StringLength(10)]
        public string? Gender { get; set; }
        [StringLength(40)]
        public string? Address { get; set; }
        [StringLength(20)]
        public string? Phone { get; set; }
        [StringLength(30)]
        public string? Email { get; set; }
        [StringLength(255)]
        public string Password { get; set; } = null!;
        [StringLength(255)]
        public string PasswordSalt { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime DateCreated { get; set; }
        [Column("UserTypeID")]
        public int UserTypeId { get; set; }

        [ForeignKey("UserTypeId")]
        [InverseProperty("Users")]
        public virtual Usertype UserType { get; set; } = null!;
        [InverseProperty("User")]
        public virtual ICollection<Debtbalance> Debtbalances { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Sale> Sales { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Warehouse> Warehouses { get; set; }
    }
}
