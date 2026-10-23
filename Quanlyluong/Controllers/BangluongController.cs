using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quanlyluong.Data;
using Quanlyluong.Models;

namespace Quanlyluong.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BangluongController : Controller
    {
        private readonly Quanlyluongnv db;

        public BangluongController(Quanlyluongnv context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            var danhSach = db.BangLuongs
                .Include(b => b.NhanVien)
                .OrderByDescending(b => b.Nam)
                .ThenByDescending(b => b.Thang)
                .ToList();

            return View(danhSach);
        }

        public IActionResult Create()
        {
            ViewBag.NhanViens = new SelectList(
                db.NhanViens.ToList(),
                "Id",
                "HoTen");

            return View(new Bangluong());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
            [Bind("NhanVienId,Thang,Nam,SoNgayCong,Thuong,KhauTru")]
    Bangluong bangluong)
        {
            ViewBag.NhanViens = new SelectList(
                db.NhanViens.ToList(),
                "Id",
                "HoTen",
                bangluong.NhanVienId);

            var nhanvien = db.NhanViens.Find(bangluong.NhanVienId);

            if (nhanvien == null)
            {
                ModelState.AddModelError(
                    "NhanVienId",
                    "Vui lòng chọn nhân viên hợp lệ");
            }

            bool daCoLuong = db.BangLuongs.Any(b =>
                b.NhanVienId == bangluong.NhanVienId &&
                b.Thang == bangluong.Thang &&
                b.Nam == bangluong.Nam);

            if (daCoLuong)
            {
                ModelState.AddModelError(
                    "",
                    "Nhân viên đã có bảng lương trong tháng này");
            }

            if (!ModelState.IsValid)
            {
                return View(bangluong);
            }

            bangluong.LuongCoBan = nhanvien!.LuongCoBan;
            bangluong.PhuCap = nhanvien.PhuCap;

            bangluong.ThucLinh = Math.Round(
                bangluong.LuongCoBan / 26 * bangluong.SoNgayCong
                + bangluong.PhuCap
                + bangluong.Thuong
                - bangluong.KhauTru,
                0,
                MidpointRounding.AwayFromZero);

            if (bangluong.ThucLinh < 0)
            {
                ModelState.AddModelError(
                    "",
                    "Khấu trừ quá lớn, lương thực lĩnh bị âm");

                return View(bangluong);
            }

            db.BangLuongs.Add(bangluong);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var bangluong = db.BangLuongs
                .Include(b => b.NhanVien)
                .FirstOrDefault(b => b.Id == id);

            if (bangluong == null)
            {
                return NotFound();
            }

            ViewBag.HoTen = bangluong.NhanVien?.HoTen;

            return View(bangluong);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(
            int id,
            [Bind("Id,NhanVienId,Thang,Nam,SoNgayCong,Thuong,KhauTru")]
    Bangluong model)
        {
            var luongCu = db.BangLuongs
                .Include(b => b.NhanVien)
                .FirstOrDefault(b => b.Id == id);

            if (luongCu == null)
            {
                return NotFound();
            }

            if (id != model.Id ||
                model.NhanVienId != luongCu.NhanVienId)
            {
                return BadRequest();
            }

            ViewBag.HoTen = luongCu.NhanVien?.HoTen;

            bool daCoLuong = db.BangLuongs.Any(b =>
                b.Id != id &&
                b.NhanVienId == model.NhanVienId &&
                b.Thang == model.Thang &&
                b.Nam == model.Nam);

            if (daCoLuong)
            {
                ModelState.AddModelError(
                    "",
                    "Nhân viên đã có bảng lương trong tháng này");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            decimal thucLinh = Math.Round(
                luongCu.LuongCoBan / 26 * model.SoNgayCong
                + luongCu.PhuCap
                + model.Thuong
                - model.KhauTru,
                0,
                MidpointRounding.AwayFromZero);

            if (thucLinh < 0)
            {
                ModelState.AddModelError(
                    "",
                    "Khấu trừ quá lớn, lương thực lĩnh bị âm");

                return View(model);
            }

            luongCu.Thang = model.Thang;
            luongCu.Nam = model.Nam;
            luongCu.SoNgayCong = model.SoNgayCong;
            luongCu.Thuong = model.Thuong;
            luongCu.KhauTru = model.KhauTru;
            luongCu.ThucLinh = thucLinh;

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var bangluong = db.BangLuongs
                .Include(b => b.NhanVien)
                .FirstOrDefault(b => b.Id == id);

            if (bangluong == null)
            {
                return NotFound();
            }

            return View(bangluong);
        }

        
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var bangluong = db.BangLuongs.Find(id);

            if (bangluong == null)
            {
                return NotFound();
            }

            db.BangLuongs.Remove(bangluong);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}