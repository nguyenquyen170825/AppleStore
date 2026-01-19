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

        public DbSet<SanPham> SanPham { get; set; }
    }
}