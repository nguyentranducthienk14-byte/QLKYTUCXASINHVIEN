using QLKYTUCXASINHVIEN.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace QLKYTUCXASINHVIEN.Controllers
{
    public class DangKyController : Controller
    {
        private KTXContext db = new KTXContext();

        public ActionResult Index()
        {
            
            var dsDangKy = db.DangKys.Include("SinhVien").Include("Phong").ToList();

            return View(dsDangKy);
        }

       
        //  DangKy/Create
        public ActionResult Create()
        {
            // 1. Lấy danh sách sinh viên
            ViewBag.MaSV = new SelectList(db.SinhViens, "MaSV", "HoTen");

            // 2. Lấy danh sách phòng và xử lý lỗi Where/Select
            var phongTrong = db.Phongs
                .AsEnumerable() // Đưa dữ liệu về bộ nhớ để tránh lỗi định dạng chuỗi của LINQ to Entities
                .Where(p => p.DangO < p.SucChua) // Lọc phòng còn chỗ
                .Select(p => new {
                    MaPhong = p.MaPhong,
                    HienThi = string.Format("{0} - {1} (Trống {2} chỗ)",
                                            p.ToAnha,
                                            p.SoPhong,
                                            (p.SucChua - p.DangO))
                }).ToList();

            ViewBag.MaPhong = new SelectList(phongTrong, "MaPhong", "HienThi");

            return View();
        }

        // POST: DangKy/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Thêm NgayBatDau, NgayKetThuc vào Bind
        public ActionResult Create([Bind(Include = "MaSV,MaPhong,NgayDangKy,NgayBatDau,NgayKetThuc,TrangThai")] DangKy dangKy)
        {
            if (ModelState.IsValid)
            {
                // 1. Xử lý ngày đăng ký
                if (dangKy.NgayDangKy == DateTime.MinValue)
                    dangKy.NgayDangKy = DateTime.Now;

                // 2. Nếu NgayBatDau bị trống, gán là hôm nay để tránh lỗi SQL
                if (dangKy.NgayBatDau == null)
                    dangKy.NgayBatDau = DateTime.Now;

                // 3. Mặc định trạng thái
                if (string.IsNullOrEmpty(dangKy.TrangThai))
                    dangKy.TrangThai = "Chờ duyệt";

                db.DangKys.Add(dangKy);

                // CẦN THÊM: Tăng số người ở trong phòng khi tạo đơn mới
                var phong = db.Phongs.Find(dangKy.MaPhong);
                if (phong != null)
                {
                    phong.DangO++;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Nếu lỗi, nạp lại danh sách chọn
            ViewBag.MaSV = new SelectList(db.SinhViens, "MaSV", "HoTen", dangKy.MaSV);
            ViewBag.MaPhong = new SelectList(db.Phongs, "MaPhong", "SoPhong", dangKy.MaPhong);
            return View(dangKy);
        
        }
        // GET: DangKy/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            DangKy dangKy = db.DangKys.Find(id);
            if (dangKy == null) return HttpNotFound();

            ViewBag.MaSV = new SelectList(db.SinhViens, "MaSV", "HoTen", dangKy.MaSV);
            ViewBag.MaPhong = new SelectList(db.Phongs, "MaPhong", "SoPhong", dangKy.MaPhong);
            return View(dangKy);
        }

        // POST: DangKy/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Edit([Bind(Include = "MaDangKy,MaSV,MaPhong,NgayDangKy,NgayBatDau,NgayKetThuc,TrangThai")] DangKy dangKy)
        {
            if (ModelState.IsValid)
            {
                // Lấy dữ liệu cũ trong DB để so sánh phòng
                var oldData = db.DangKys.AsNoTracking().FirstOrDefault(d => d.MaDangKy == dangKy.MaDangKy);

                if (oldData != null && oldData.MaPhong != dangKy.MaPhong)
                {
                    // Trừ 1 người ở phòng cũ
                    var oldRoom = db.Phongs.Find(oldData.MaPhong);
                    if (oldRoom != null) oldRoom.DangO--;

                    // Cộng 1 người vào phòng mới
                    var newRoom = db.Phongs.Find(dangKy.MaPhong);
                    if (newRoom != null) newRoom.DangO++;
                }

                db.Entry(dangKy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dangKy);
        }

        // GET: DangKy/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            DangKy dangKy = db.DangKys.Include(d => d.SinhVien).Include(d => d.Phong).FirstOrDefault(d => d.MaDangKy == id);
            if (dangKy == null) return HttpNotFound();
            return View(dangKy);
        }

        // POST: DangKy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DangKy dangKy = db.DangKys.Find(id);

            // Khi xóa đơn, tự động trừ 1 người ở phòng đó
            var phong = db.Phongs.Find(dangKy.MaPhong);
            if (phong != null && phong.DangO > 0) phong.DangO--;

            db.DangKys.Remove(dangKy);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}