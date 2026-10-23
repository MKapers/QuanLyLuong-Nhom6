using System.ComponentModel.DataAnnotations;

namespace Quanlyluong.Models
{
    public class Phongban
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui long nhap ten phong ban")]
        [StringLength(100)]
        [Display(Name = "Ten phong ban")]
        public string TenPhong { get; set; } = "";

        [Display(Name = "Ghi chu")]
        public string? GhiChu { get; set; }
    }
}