using Microsoft.EntityFrameworkCore;
using DUANCUAHANGAPPLE.Models;

namespace DUANCUAHANGAPPLE.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users {get; set;}
        public DbSet<DanhMuc> DanhMucs { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<BienTheSanPham> BienTheSanPhams { get; set; }
        public DbSet<ThongSoKyThuat> ThongSoKyThuats { get; set; }
        public DbSet<HinhAnh> HinhAnhs { get; set; }
        public DbSet<GioHang> GioHangs {get; set;}
        public DbSet<New> News {get;set;}
        public DbSet<EmailOtp> EmailOtps {get; set;}
        public DbSet<PendingUser> PendingUsers {get; set;}
        public DbSet<ThanhToan> ThanhToans { get; set; }
        public DbSet<ChiTietThanhToan> ChiTietThanhToans { get; set; }
        public DbSet<LoaiThongSo> LoaiThongSos { get; set; }
        public DbSet<PhieuGiamGia> PhieuGiamGias { get; set; }
        public DbSet<PhieuGiamGiaNguoiDung> PhieuGiamGiaNguoiDungs { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);
            
            // Cấu hình độ chính xác cho các cột tiền tệ (Decimal)
            modelBuilder.Entity<BienTheSanPham>().Property(b => b.Gia).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<BienTheSanPham>().Property(b => b.GiaCu).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<BienTheSanPham>().Property(b => b.GiamGia).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<ChiTietThanhToan>().Property(c => c.DonGia).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<ThanhToan>().Property(t => t.TongTien).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<ThanhToan>().Property(t => t.SoTienGiam).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<User>().Property(u => u.TongTienDaMua).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<PhieuGiamGia>().Property(p => p.GiaTriGiam).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<PhieuGiamGia>().Property(p => p.DonHangToiThieu).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<PhieuGiamGia>().Property(p => p.GiamToiDa).HasColumnType("decimal(18,2)");

            // Chống trùng link tin tức
            modelBuilder.Entity<New>().HasIndex(n=>n.Link).IsUnique();
        }
    }
}