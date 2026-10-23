using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quanlyluong.Models
{
    public class Bangluong
    {
        [Key]
        public int Id { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Vui lòng chọn nhân viên")]
        [Display(Name = "Nhân viên")]
        public int NhanVienId { get; set; }

        public Nhanvien? NhanVien { get; set; }

        [Range(1, 12)]
        [Display(Name = "Tháng")]
        public int Thang { get; set; } = DateTime.Today.Month;

        [Range(2020, 2100)]
        [Display(Name = "Năm")]
        public int Nam { get; set; } = DateTime.Today.Year;

        [Range(0, 26,
            ErrorMessage = "Ngày công từ 0 đến 26")]
        [Display(Name = "Số ngày công")]
        public int SoNgayCong { get; set; } = 26;

        [Column(TypeName = "decimal(18,0)")]
        [Display(Name = "Lương cơ bản")]
        public decimal LuongCoBan { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        [Display(Name = "Phụ cấp")]
        public decimal PhuCap { get; set; }

        [Range(0, 100000000)]
        [Column(TypeName = "decimal(18,0)")]
        [Display(Name = "Thưởng")]
        public decimal Thuong { get; set; }

        [Range(0, 100000000)]
        [Column(TypeName = "decimal(18,0)")]
        [Display(Name = "Khấu trừ")]
        public decimal KhauTru { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        [Display(Name = "Thực lĩnh")]
        public decimal ThucLinh { get; set; }
    }
}