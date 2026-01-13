using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;

namespace QLKYTUCXASINHVIEN.Models
{
    [Table("SinhVien")]
    public class SinhVien
    {
        [Key]
        public string MaSV { get; set; }
        public string HoTen { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string CCCD { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string Khoa { get; set; }
        public string Lop { get; set; }
        public string AnhDaiDien { get; set; }
        public string TrangThai { get; set; }
        public DateTime? NgayDangKy { get; set; }
        public string MaPhong { get; set; } // Khóa ngoại lưu mã phòng (P101, P102...)
       
        [ForeignKey("MaPhong")]
        // PHẢI SỬA: Thay PhoneAttribute bằng chữ Phong (tên class của bảng Phòng)
        public virtual Phong Phong { get; set; }
    }
}