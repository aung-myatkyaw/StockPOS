using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockPOS.Models
{
    public class SearchModels
    {
        [StringLength(50)]
        [Required]
        public string SearchString { get; set; } = null!;


    }
}

