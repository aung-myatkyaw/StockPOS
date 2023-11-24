using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockPOS.Models.ViewModels
{
    public class ProductCategoryUpdateModel
    {
        [StringLength(50)]
        [Required]
        public string CategoryId { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string? CategoryName { get; set; }

        [StringLength(100)]
        public string? Description { get; set; }

    }
}

