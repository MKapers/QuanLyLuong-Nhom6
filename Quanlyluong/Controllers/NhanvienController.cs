using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quanlyluong.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Quanlyluong.Models;

namespace Quanlyluong.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NhanvienController : Controller
    {
        private readonly Quanlyluongnv db;

        public NhanvienController(Quanlyluongnv context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            var danhSach = db.NhanViens
                .Include(n => n.PhongBan)
                .ToList();

            return View(danhSach);
        }

        public IActionResult Create()
        {
            ViewBag.PhongBans = new SelectList(
                db.PhongBans.ToList(),
                "Id",
                "TenPhong");

            return View(new Nhanvien());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
    [Bind("MaNV,HoTen,PhongBanId,LuongCoBan,PhuCap")]
    Nhanvien nhanvien)
        {
            ViewBag.PhongBans = new SelectList(
                db.PhongBans.ToList(),
                "Id",
                "TenPhong",
                nhanvien.PhongBanId);

            if (db.NhanViens.Any(n => n.MaNV == nhanvien.MaNV))
            {
                ModelState.AddModelError(
                    "MaNV",
                    "Mã nhân viên đã tồn tại");
            }

            if (!db.PhongBans.Any(p => p.Id == nhanvien.PhongBanId))
            {
                ModelState.AddModelError(
                    "PhongBanId",
                    "Vui lòng chọn phòng ban hợp lệ");
            }

            if (!ModelState.IsValid)
            {
                return View(nhanvien);
            }

            db.NhanViens.Add(nhanvien);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var nhanvien = db.NhanViens.Find(id);

            if (nhanvien == null)
            {
                return NotFound();
            }

            ViewBag.PhongBans = new SelectList(
                db.PhongBans.ToList(),
                "Id",
                "TenPhong",
                nhanvien.PhongBanId);

            return View(nhanvien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(
    int id,
    [Bind("Id,MaNV,HoTen,PhongBanId,LuongCoBan,PhuCap")]
    Nhanvien nhanvien)
        {
            if (id != nhanvien.Id)
            {
                return BadRequest();
            }

            ViewBag.PhongBans = new SelectList(
                db.PhongBans.ToList(),
                "Id",
                "TenPhong",
                nhanvien.PhongBanId);

            if (db.NhanViens.Any(
                n => n.Id != id && n.MaNV == nhanvien.MaNV))
            {
                ModelState.AddModelError(
                    "MaNV",
                    "Mã nhân viên đã tồn tại");
            }

            if (!db.PhongBans.Any(p => p.Id == nhanvien.PhongBanId))
            {
                ModelState.AddModelError(
                    "PhongBanId",
                    "Vui lòng chọn phòng ban hợp lệ");
            }

            if (!ModelState.IsValid)
            {
                return View(nhanvien);
            }

            var nhanvienCu = db.NhanViens.Find(id);

            if (nhanvienCu == null)
            {
                return NotFound();
            }

            nhanvienCu.MaNV = nhanvien.MaNV;
            nhanvienCu.HoTen = nhanvien.HoTen;
            nhanvienCu.PhongBanId = nhanvien.PhongBanId;
            nhanvienCu.LuongCoBan = nhanvien.LuongCoBan;
            nhanvienCu.PhuCap = nhanvien.PhuCap;

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var nhanvien = db.NhanViens
                .Include(n => n.PhongBan)
                .FirstOrDefault(n => n.Id == id);

            if (nhanvien == null)
            {
                return NotFound();
            }

            return View(nhanvien);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var nhanvien = db.NhanViens
                .Include(n => n.PhongBan)
                .FirstOrDefault(n => n.Id == id);

            if (nhanvien == null)
            {
                return NotFound();
            }

            if (db.BangLuongs.Any(b => b.NhanVienId == id))
            {
                ModelState.AddModelError(
                    "",
                    "Nhân viên đã có bảng lương nên không thể xóa");

                return View("Delete", nhanvien);
            }

            if (db.TaiKhoans.Any(t => t.NhanVienId == id))
            {
                ModelState.AddModelError(
                    "",
                    "Nhân viên đang có tài khoản nên không thể xóa.");

                return View("Delete", nhanvien);
            }

            db.NhanViens.Remove(nhanvien);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}