using QLKYTUCXASINHVIEN.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace QLKYTUCXASINHVIEN.Controllers
{
    public class HoaDonController : Controller
    {
        private KTXContext db = new KTXContext();

        public ActionResult Index(string searchString)
        {
            // 1. Lấy danh sách hóa đơn từ DB
            var hoadons = db.HoaDon.AsQueryable();

            // 2. Chức năng tìm kiếm theo Mã SV hoặc Mã Phòng
            if (!string.IsNullOrEmpty(searchString))
            {
                hoadons = hoadons.Where(s => s.MaSV.Contains(searchString) || s.MaPhong.Contains(searchString));
            }

            // 3. Tính toán nhanh số liệu cho các thẻ Card
            ViewBag.TongDoanhThu = hoadons.Where(h => h.TrangThai == "Đã thanh toán").Sum(h => (decimal?)h.TongTien) ?? 0;
            ViewBag.ChuaThu = hoadons.Where(h => h.TrangThai == "Chưa thanh toán").Sum(h => (decimal?)h.TongTien) ?? 0;

            return View(hoadons.OrderByDescending(h => h.NgayTao).ToList());
        }

        // GET: HoaDon/Create
        public ActionResult Create()
        {
            // 1. Phải tính toán lại Doanh thu để hiển thị lên Header của trang Create
            ViewBag.DoanhThu = db.HoaDon
                .Where(h => h.TrangThai == "Đã thanh toán")
                .Sum(h => (decimal?)h.TongTien) ?? 0;

            // 2. Load danh sách Sinh viên/Phòng cho DropdownList (nếu có)
            ViewBag.MaSV = new SelectList(db.SinhViens, "MaSV", "HoTen");
            ViewBag.MaPhong = new SelectList(db.Phongs, "MaPhong", "SoPhong");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                // 1. Gán ngày và hạn thanh toán
                hoaDon.NgayTao = DateTime.Now;
                hoaDon.HanThanhToan = DateTime.Now.AddDays(7);

                // 2. Xử lý các trường tiền (Dùng toán tử 3 ngôi để hết gạch đỏ)
                hoaDon.TienPhong = hoaDon.TienPhong > 0 ? hoaDon.TienPhong : 0;
                hoaDon.TienDien = hoaDon.TienDien > 0 ? hoaDon.TienDien : 0;
                hoaDon.TienNuoc = hoaDon.TienNuoc > 0 ? hoaDon.TienNuoc : 0;

                // 3. QUAN TRỌNG: Tự tạo Số hóa đơn để tránh lỗi UNIQUE KEY
                if (string.IsNullOrEmpty(hoaDon.SoHoaDon))
                {
                    // Tạo mã dạng HD + thời gian (ví dụ: HD202601121430)
                    hoaDon.SoHoaDon = "HD" + DateTime.Now.ToString("yyyyMMddHHmmss");
                }

                db.HoaDon.Add(hoaDon);
                db.SaveChanges(); // Lệnh này sẽ không còn lỗi image_ad37ea.png nữa

                return RedirectToAction("Index");
            }
            return View(hoaDon);
        }
    }
    
    
}
