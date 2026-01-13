using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLKYTUCXASINHVIEN.Models
{

    [Table("DangKy")] // Tên bảng trong SQL phải chính xác là DangKy
    public class DangKy
    {
        [Key] // Khóa chính - Sửa lỗi "EntityType has no key defined"
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaDangKy { get; set; }

        [Required]
        public string MaSV { get; set; }

        [Required]
        public string MaPhong { get; set; }

        public DateTime NgayDangKy { get; set; }

        // Dùng DateTime? (có dấu chấm hỏi) để cho phép null nếu SQL cho phép
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }

        public string TrangThai { get; set; }

        // --- LIÊN KẾT DỮ LIỆU (Navigation Properties) ---

        [ForeignKey("MaSV")]
        public virtual SinhVien SinhVien { get; set; }

        [ForeignKey("MaPhong")]
        public virtual Phong Phong { get; set; }
        
        
    }
}