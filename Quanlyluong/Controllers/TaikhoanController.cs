using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quanlyluong.Data;
using Quanlyluong.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Quanlyluong.Controllers
{
    public class TaikhoanController : Controller
    {
        private readonly Quanlyluongnv db;

        public TaikhoanController(Quanlyluongnv context)
        {
            db = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult DangNhap()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new Dangnhap());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangNhap(Dangnhap model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var taiKhoan = db.TaiKhoans.FirstOrDefault(t =>
                t.TenDangNhap == model.TenDangNhap.Trim());

            if (taiKhoan == null)
            {
                ModelState.AddModelError(
                    "", "Tên đăng nhập hoặc mật khẩu không đúng.");

                return View(model);
            }

            var bamMatKhau = new PasswordHasher<Taikhoan>();

            var ketQua = bamMatKhau.VerifyHashedPassword(
                taiKhoan,
                taiKhoan.MatKhauHash,
                model.MatKhau);

            if (ketQua == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(
                    "", "Tên đăng nhập hoặc mật khẩu không đúng.");

                return View(model);
            }

            if (taiKhoan.Quyen != "Admin" &&
                taiKhoan.Quyen != "NhanVien")
            {
                ModelState.AddModelError(
                    "", "Tài khoản chưa được cấp quyền hợp lệ.");

                return View(model);
            }

            if (taiKhoan.Quyen == "NhanVien" &&
                !db.NhanViens.Any(n => n.Id == taiKhoan.NhanVienId))
            {
                ModelState.AddModelError(
                    "", "Tài khoản chưa gắn với nhân viên hợp lệ.");

                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    taiKhoan.Id.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    taiKhoan.TenDangNhap),

                new Claim(
                    ClaimTypes.Role,
                    taiKhoan.Quyen)
            };

            if (taiKhoan.NhanVienId.HasValue)
            {
                claims.Add(new Claim(
                    "NhanVienId",
                    taiKhoan.NhanVienId.Value.ToString()));
            }

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            HttpContext.Session.SetString(
    "TenDangNhap", taiKhoan.TenDangNhap);

            if (taiKhoan.Quyen == "NhanVien")
            {
                return RedirectToAction("LuongCuaToi");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Clear();

            return RedirectToAction("DangNhap");
        }

        public IActionResult KhongCoQuyen()
        {
            Response.StatusCode = 403;

            return View();
        }


        [Authorize(Roles = "NhanVien")]
        public IActionResult LuongCuaToi()
        {
            var maNhanVien = User.FindFirst("NhanVienId")?.Value;

            if (!int.TryParse(maNhanVien, out int nhanVienId))
            {
                return Forbid();
            }

            var danhSach = db.BangLuongs
                .Include(b => b.NhanVien)
                .Where(b => b.NhanVienId == nhanVienId)
                .OrderByDescending(b => b.Nam)
                .ThenByDescending(b => b.Thang)
                .ToList();

            return View(danhSach);
        }
    }
}