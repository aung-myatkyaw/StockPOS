using System;
using System.ComponentModel.DataAnnotations;

namespace StockPOS.Models
{
    public record UserRegisterDTO(
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
        string Password,
        [Required]
        int UserTypeId
    );

    public record UserDTO(
        int Id,
        string FullName,
        string UserName,
        DateTime? DateofBirth,
        string Gender,
        string Address,
        string Phone,
        string Email,
        int UserTypeId
    );

    public record UserLoginDTO(
        [Required]
        string UserName,
        [Required]
        string Password
    );
}