using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanlyluong.Data;
using Quanlyluong.Models;

namespace Quanlyluong.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PhongbanController : Controller
    {
        private readonly Quanlyluongnv db;

        public PhongbanController(Quanlyluongnv context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            var danhSach = db.PhongBans.ToList();

            return View(danhSach);
        }

        public IActionResult Create()
        {
            return View(new Phongban());
        }
        public IActionResult Edit(int id)
        {
            var phongban = db.PhongBans.Find(id);

            if (phongban == null)
            {
                return NotFound();
            }

            return View(phongban);
        }

        public IActionResult Delete(int id)
        {
            var phongban = db.PhongBans.Find(id);

            if (phongban == null)
            {
                return NotFound();
            }

            return View(phongban);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
    [Bind("TenPhong,GhiChu")] Phongban phongban)
        {
            if (!ModelState.IsValid)
            {
                return View(phongban);
            }

            db.PhongBans.Add(phongban);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(
    int id,
    [Bind("Id,TenPhong,GhiChu")] Phongban phongban)
        {
            if (id != phongban.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(phongban);
            }

            var phongCu = db.PhongBans.Find(id);

            if (phongCu == null)
            {
                return NotFound();
            }

            phongCu.TenPhong = phongban.TenPhong;
            phongCu.GhiChu = phongban.GhiChu;

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var phongban = db.PhongBans.Find(id);

            if (phongban == null)
            {
                return NotFound();
            }
            if (db.NhanViens.Any(n => n.PhongBanId == id))
            {
                ModelState.AddModelError(
                    "",
                    "Phòng ban còn nhân viên nên không thể xóa");

                return View("Delete", phongban);
            }
            db.PhongBans.Remove(phongban);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}