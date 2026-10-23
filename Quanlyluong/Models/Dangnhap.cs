using System.ComponentModel.DataAnnotations;

namespace Quanlyluong.Models
{
    public class Dangnhap
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        public string TenDangNhap { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; } = "";
    }
}