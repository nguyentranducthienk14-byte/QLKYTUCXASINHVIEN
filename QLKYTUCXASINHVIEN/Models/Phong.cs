using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLKYTUCXASINHVIEN.Models
{
    [Table("Phong")]
    public class Phong
    {
        [Key]
        public string MaPhong { get; set; }
        public string ToAnha { get; set; }
        public string SoPhong { get; set; }
        public string LoaiPhong { get; set; }
        public int SucChua { get; set; }
        public int DangO { get; set; }

        // Đổi DonGia thành GiaThue để hết lỗi "Invalid column name 'DonGia'"
        public decimal GiaThue { get; set; }

        public string TinhTrang { get; set; }
        public DateTime NgayTao { get; internal set; }
    }
}