using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockPOS.Models
{
    [Table("villages")]
    [Index("GroupVillageId", Name = "VillageToGroupVillage_idx")]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public partial class Village
    {
        [Key]
        [Column("VillageID")]
        public int VillageId { get; set; }
        [StringLength(45)]
        public string? VillageName { get; set; }
        [StringLength(45)]
        public string? VillageShortName { get; set; }
        [Column("GroupVillageID")]
        public int? GroupVillageId { get; set; }

        [ForeignKey("GroupVillageId")]
        [InverseProperty("Villages")]
        public virtual Groupvillage? GroupVillage { get; set; }
    }
}
