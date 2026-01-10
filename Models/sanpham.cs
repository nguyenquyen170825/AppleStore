namespace DUANCUAHANGAPPLE.Models
{
    public class SanPham
    {
        public int Id { get; set; }
        public string Ten { get; set; } = string.Empty;
        public decimal Gia { get; set; }
        public decimal GiaGiam { get; set; } 
        public string HinhAnh { get; set; } = string.Empty;
        
    }
}