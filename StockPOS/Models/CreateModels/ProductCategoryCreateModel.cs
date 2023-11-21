using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockPOS.Models.CreateModels
{
	public class ProductCategoryCreateModel
	{
        [Required]
        [StringLength(100)]
        public string? CategoryName { get; set; }

        [StringLength(100)]
        public string? Description { get; set; }
        
    }
}

