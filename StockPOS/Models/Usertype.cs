using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockPOS.Models
{
    [Table("usertype")]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public partial class Usertype
    {
        public Usertype()
        {
            Users = new HashSet<User>();
        }

        [Key]
        [Column("UserTypeID")]
        public int UserTypeId { get; set; }
        [StringLength(45)]
        public string? UserTypeName { get; set; }

        [InverseProperty("UserType")]
        public virtual ICollection<User> Users { get; set; }
    }
}
