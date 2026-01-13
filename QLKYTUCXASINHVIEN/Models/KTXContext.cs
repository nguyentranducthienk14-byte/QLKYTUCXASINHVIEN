
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions; // Dòng này cực kỳ quan trọng

namespace QLKYTUCXASINHVIEN.Models
{
    public class KTXContext : DbContext
    {

        // "name=KTXContext" phải trùng khớp với file Web.config của bạn
        public KTXContext() : base("name=KTXContext")
        {
        }

        // Tên SinhViens này phải viết đúng để Controller gọi được
       
        public virtual DbSet<SinhVien> SinhViens { get; set; }


        // THÊM DÒNG NÀY ĐỂ HẾT LỖI GẠCH ĐỎ
        public virtual DbSet<Phong> Phongs { get; set; }
        public virtual DbSet<DangKy> DangKys { get; set; }
        public virtual DbSet<HoaDon> HoaDon { get; set; }
        public object Phong { get; internal set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Ngăn Entity Framework tự động thêm số nhiều (s) vào tên bảng SQL
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }


    }
}