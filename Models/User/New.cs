using System.ComponentModel.DataAnnotations;

namespace DUANCUAHANGAPPLE.Models
{
    public class New
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Tieude { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        public string Motangan { get; set; }
        [Required]
        public DateTime Ngaytao { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string Source { get; set; }
    }
}