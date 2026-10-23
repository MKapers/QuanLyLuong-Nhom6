using System.ComponentModel.DataAnnotations;

namespace Quanlyluong.Models
{
    public class Taikhoan
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string TenDangNhap { get; set; } = "";

        [Required]
        [StringLength(500)]
        public string MatKhauHash { get; set; } = "";

        [Required]
        [StringLength(20)]
        public string Quyen { get; set; } = "";

        public int? NhanVienId { get; set; }

        public Nhanvien? NhanVien { get; set; }
    }
}