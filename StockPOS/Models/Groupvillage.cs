using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockPOS.Models
{
    [Table("groupvillages")]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public partial class Groupvillage
    {
        public Groupvillage()
        {
            Villages = new HashSet<Village>();
        }

        [Key]
        [Column("GroupVillageID")]
        public int GroupVillageId { get; set; }
        [StringLength(45)]
        public string? GroupVillageName { get; set; }
        [StringLength(45)]
        public string? GroupVillageShortName { get; set; }
        [StringLength(45)]
        public string? OtherColumn1 { get; set; }

        [InverseProperty("GroupVillage")]
        public virtual ICollection<Village> Villages { get; set; }
    }
}
