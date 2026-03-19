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
        
    
    }
}