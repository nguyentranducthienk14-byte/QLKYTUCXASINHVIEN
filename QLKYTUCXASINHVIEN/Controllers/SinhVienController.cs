using QLKYTUCXASINHVIEN.Models;
using System;
using System.Data.Entity; // Phải có dòng này để dùng .ToList()
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace QLKYTUCXASINHVIEN.Controllers
{
    public class SinhVienController : Controller
    {
        private KTXContext db = new KTXContext();

        public ActionResult Index()
        {
            // Lấy dữ liệu từ DB
            var danhSach = db.SinhViens.ToList();

            // Gán vào ViewBag đúng tên mà View đang gọi
            ViewBag.DanhSachSinhVien = danhSach;

            return View(danhSach);
        }
        // 1. Hàm để mở trang nhập liệu (Giao diện)
        // 1. Hiện form để nhập liệu
        public ActionResult Create()
        {
            return View();
        }

        // 2. Nhận dữ liệu từ form và lưu vào database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SinhVien sv)
        {
            if (ModelState.IsValid)
            {
                db.SinhViens.Add(sv); // Thêm sv mới vào danh sách
                db.SaveChanges();      // Lưu xuống SQL Server
                return RedirectToAction("Index"); // Lưu xong quay về trang danh sách
            }
            return View(sv);
        }

        public ActionResult Delete(string id)
        {
            // Tìm sinh viên theo mã
            var sv = db.SinhViens.Find(id);
            if (sv != null)
            {
                db.SinhViens.Remove(sv); // Xóa khỏi bộ nhớ
                db.SaveChanges();         // Lưu thay đổi xuống SQL
            }
            return RedirectToAction("Index"); // Xóa xong tải lại trang
        }

        // 1. Hàm GET: Hiển thị form edit
        // Hàm GET: Edit
        public ActionResult Edit(string id)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            var sv = db.SinhViens.Find(id);
            if (sv == null) { return HttpNotFound(); }

            // 1. Lấy danh sách phòng còn chỗ (So sánh DangO và SucChua từ SQL)
            var dsPhongConTrong = db.Phongs // Thêm chữ 's' vào đây
                .Where(p => p.DangO < p.SucChua)
                .ToList()
                .Select(p => new {
                    MaPhong = p.MaPhong,
                    // Hiển thị: Tòa A - Phòng 101 (Trống: 2)
                    HienThi = p.ToAnha + " - " + p.SoPhong + " (Trống: " + (p.SucChua - p.DangO) + ")"
                }).ToList();

            // 2. Gán vào ViewBag để hiển thị lên Dropdown
            ViewBag.MaPhong = new SelectList(dsPhongConTrong, "MaPhong", "HienThi", sv.MaPhong);

            return View(sv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SinhVien sv) // SinhVien sv đã bao gồm thuộc tính MaPhong
        {
            if (ModelState.IsValid)
            {
                db.Entry(sv).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // Nếu lỗi, phải nạp lại danh sách phòng cho View
            ViewBag.MaPhong = new SelectList(db.Phongs, "MaPhong", "SoPhong", sv.MaPhong);
            return View(sv);
        }
    }
}