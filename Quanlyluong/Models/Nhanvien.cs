using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quanlyluong.Models
{
    public class Nhanvien
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mã nhân viên")]
        [StringLength(15)]
        [Display(Name = "Mã nhân viên")]
        public string MaNV { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [StringLength(100)]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; } = "";

        [Range(1, int.MaxValue,
            ErrorMessage = "Vui lòng chọn phòng ban")]
        [Display(Name = "Phòng ban")]
        public int PhongBanId { get; set; }

        public Phongban? PhongBan { get; set; }

        [Range(1, 1000000000,
            ErrorMessage = "Lương cơ bản từ 1 đến 1 tỷ")]
        [Column(TypeName = "decimal(18,0)")]
        [Display(Name = "Lương cơ bản")]
        public decimal LuongCoBan { get; set; }

        [Range(0, 100000000,
            ErrorMessage = "Phụ cấp từ 0 đến 100 triệu")]
        [Column(TypeName = "decimal(18,0)")]
        [Display(Name = "Phụ cấp")]
        public decimal PhuCap { get; set; }
    }
}