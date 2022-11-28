using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace StockPOS.Models
{
    [Table("eventlog")]
    [Index(nameof(UserId), Name = "user_id")]
    public partial class Eventlog
    {
        [Key]
        [Column("ID", TypeName = "int(11)")]
        public int Id { get; set; }
        [Column(TypeName = "int(11)")]
        public EventLogType LogType { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LogDateTime { get; set; }
        [StringLength(100)]
        public string Source { get; set; }
        [StringLength(100)]
        public string FormName { get; set; }
        [Column(TypeName = "text")]
        public string LogMessage { get; set; }
        [Column(TypeName = "text")]
        public string ErrorMessage { get; set; }
        [Column("UserID", TypeName = "int(11)")]
        public int? UserId { get; set; }
    }

    public enum EventLogType
    {
        Info = 1,
        Error = 2,
        Warning = 3,
        Insert = 4,
        Update = 5,
        Delete = 6
    }
}
