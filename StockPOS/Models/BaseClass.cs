using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace StockPOS.Models
{
    [Serializable]
    public class BaseClass
    {

        [Column("CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Column("CreatedBy")]
        public string? CreatedBy { get; set; }

    }
}

