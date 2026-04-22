using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DUANCUAHANGAPPLE.Models
{
    public class EmailOtp
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public DateTime ExpireAt { get; set; }

        public int? UserId { get; set; } // Thêm liên kết với User
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}