using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
// 1. QUAN TRỌNG: Phải có dòng này để dùng được lệnh .Include
using System.Data.Entity;
using QLKYTUCXASINHVIEN.Models;

namespace QLKYTUCXASINHVIEN.Controllers
{
    public class ThanhToanController : Controller
    {
        // 2. KHAI BÁO BIẾN db: Để kết nối đến cơ sở dữ liệu
        private KTXContext db = new KTXContext();

        // GET: ThanhToan
        public ActionResult Index()
        {
            // 3. LẤY DỮ LIỆU: Kèm theo SinhVien và Phong để hiện tên thật
            // Thử bỏ .Include nếu không chắc chắn tên thuộc tính điều hướng
            var hoaDons = db.HoaDon.Include("Phong").OrderByDescending(h => h.NgayTao).ToList();

            // 4. THỐNG KÊ: Sử dụng (decimal?) và ?? 0 để tránh lỗi "Value cannot be null"
            ViewBag.TongDaThu = hoaDons.Where(h => h.TrangThai == "Đã thanh toán").Sum(h => (decimal?)h.TongTien) ?? 0;
            ViewBag.ChuaThu = hoaDons.Where(h => h.TrangThai != "Đã thanh toán").Sum(h => (decimal?)h.TongTien) ?? 0;

            return View(hoaDons);
        }

        // Giải phóng bộ nhớ
        protected override void Dispose(bool disposing)
        {
            if (disposing) { db.Dispose(); }
            base.Dispose(disposing);
        }
    }
}