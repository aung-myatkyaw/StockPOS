using System;
using System.ComponentModel.DataAnnotations;

namespace StockPOS.Models
{
    public record CashierRegisterDTO(
        int Id,
        [StringLength(50)]
        string FullName,
        [Required]
        [StringLength(50)]
        string UserName,
        DateTime? DateofBirth,
        [StringLength(10)]
        string Gender,
        [StringLength(40)]
        string Address,
        [StringLength(20)]
        string Phone,
        [StringLength(30)]
        string Email,
        [Required]
        [StringLength(255)]
        string Password
    );

    public record CashierDTO(
        int Id,
        string FullName,
        string UserName,
        DateTime? DateofBirth,
        string Gender,
        string Address,
        string Phone,
        string Email
    );

    public record CashierLoginDTO(
        [Required]
        string UserName,
        [Required]
        string Password
    );
}