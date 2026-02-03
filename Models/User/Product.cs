using System.ComponentModel.DataAnnotations;

namespace DUANCUAHANGAPPLE.Models
{
    public class Product
    {
        [Key]
        public int Ma { get; set; }
        public string Ten { get; set; } = string.Empty;
        public decimal Gia { get; set; }
        public decimal GiaCu { get; set; } 
        public int GiamGia { get; set; }
        public string HinhAnh { get; set; } = string.Empty;
        public int Soluong { get; set; }
        
        
    }
}