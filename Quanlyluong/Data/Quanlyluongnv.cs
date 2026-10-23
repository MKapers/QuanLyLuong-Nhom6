using Microsoft.EntityFrameworkCore;
using Quanlyluong.Models;

namespace Quanlyluong.Data
{
    public class Quanlyluongnv : DbContext
    {
        public Quanlyluongnv(
            DbContextOptions<Quanlyluongnv> options)
            : base(options)
        {
        }

        public DbSet<Phongban> PhongBans { get; set; }
        public DbSet<Nhanvien> NhanViens { get; set; }
        public DbSet<Bangluong> BangLuongs { get; set; }
        public DbSet<Taikhoan> TaiKhoans { get; set; }
    }
}
