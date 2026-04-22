using System.Net;
using System.Net.Mail;

namespace DUANCUAHANGAPPLE.Services
{
    public class SendEmailService
    {
        public async Task SendOtpEmail(string email, string otp)
        {
            var from = "quyenketvll@gmail.com";
            var appPassword = "erxu zeey evip wqbi";
            var message = new MailMessage();
            message.From = new MailAddress(from);
            message.To.Add(email); 
            message.Subject = "Mã OTP xác nhận đăng ký";
            message.Body = $"Mã OTP VayNhanh của bạn là: {otp} (có hiệu lực 5 phút)";
            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential(from, appPassword);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }
        }
    }
}