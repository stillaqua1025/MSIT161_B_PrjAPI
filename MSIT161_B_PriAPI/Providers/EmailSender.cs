using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace MSIT161_B_PriAPI.Models
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var mail = new MailMessage();
            // 設置寄件人
            mail.From = new MailAddress("one2clothesplatform@gmail.com");  // 替換為你的 Gmail 地址
            mail.To.Add(email);  // 收件人的 Email
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = htmlMessage;

            // 使用 Gmail 的 SMTP 伺服器
            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.Port = 587;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("one2clothesplatform@gmail.com", "szafyfxdbpjhuxtg"); // 使用你的 Gmail 帳號和應用程式密碼
            client.EnableSsl = true;

            // 發送郵件
            await client.SendMailAsync(mail);
        }
    }
}
