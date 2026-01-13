using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLKYTUCXASINHVIEN.Models
{
    [Table("HoaDon")]
    public class HoaDon
    {
        [Key] // Sửa lỗi "no key defined"
        public int MaHoaDon { get; set; }

        public string MaSV { get; set; }
        public string MaPhong { get; set; }
        public string SoHoaDon { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? HanThanhToan { get; set; }
        public DateTime? NgayThanhToan { get; set; }

        public decimal TienPhong { get; set; }
        public decimal TienDien { get; set; }
        public decimal TienNuoc { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }
        //public object SinhVien { get; internal set; }
        //public object Phong { get; internal set; }
        // Thêm vào trong class HoaDon để EF hiểu mối liên kết
        public virtual SinhVien SinhVien { get; set; }
        public virtual Phong Phong { get; set; }
    }
}