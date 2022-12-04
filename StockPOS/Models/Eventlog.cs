using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StockPOS.Models
{
    [Table("eventlog")]
    [Index("UserId", Name = "user_id")]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public partial class Eventlog
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        /// <summary>
        /// Info = 1, Error = 2,Warning = 3, Insert = 4,Update = 5, Delete = 6
        /// </summary>
        public EventLogType LogType { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LogDateTime { get; set; }
        [StringLength(100)]
        public string? Source { get; set; }
        [StringLength(100)]
        public string? FormName { get; set; }
        [Column(TypeName = "text")]
        public string? LogMessage { get; set; }
        [Column(TypeName = "text")]
        public string? ErrorMessage { get; set; }
        [Column("UserID")]
        public int? UserId { get; set; }
    }
}
