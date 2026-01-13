using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLKYTUCXASINHVIEN.Models; // Đảm bảo có dòng này
using System.Data.Entity; // Để dùng được hàm Include

namespace QLKYTUCXASINHVIEN.Controllers
{
    public class HomeController : Controller
    {
        // QUAN TRỌNG: Phải khởi tạo db thì mới lấy được dữ liệu
        private KTXContext db = new KTXContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BaoCao()
        {
            // 2. TÍNH TOÁN VÀ GÁN GIÁ TRỊ CHO VIEW
            ViewBag.TongSV = db.SinhViens.Count();
            ViewBag.TongPhong = db.Phongs.Count();

            // Tính doanh thu an toàn (nếu null thì bằng 0)
            ViewBag.DoanhThu = db.HoaDon
                .Where(h => h.TrangThai == "Đã thanh toán")
                .Sum(h => (decimal?)h.TongTien) ?? 0;

            ViewBag.TienNo = db.HoaDon
                .Where(h => h.TrangThai == "Chưa thanh toán")
                .Sum(h => (decimal?)h.TongTien) ?? 0;


            // Lấy danh sách đăng ký hiển thị bảng
            // Thêm .Include để "kéo" dữ liệu tên sinh viên sang cùng
            var dsDangKy = db.DangKys.Include("SinhVien").OrderByDescending(x => x.NgayDangKy).Take(5).ToList();
            return View(dsDangKy);
        }
        

        public ActionResult About()
        {
            ViewBag.Message = "Hệ thống quản lý Ký túc xá Sinh viên";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Thông tin hỗ trợ kỹ thuật";
            return View();
        }
    }
}