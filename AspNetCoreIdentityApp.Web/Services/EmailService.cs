using AspNetCoreIdentityApp.Web.OptionsModels;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AspNetCoreIdentityApp.Web.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly SmtpClient _smtpClient;

        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
            _smtpClient = new SmtpClient();
            _smtpClient.Host=_emailSettings.Host;
        }
        public async Task SendPasswordResetEmail(string resetPasswordLink, string to)
        {
            
            _smtpClient.DeliveryMethod=SmtpDeliveryMethod.Network;
            _smtpClient.UseDefaultCredentials = false;
            _smtpClient.Port = 587;
            _smtpClient.Credentials = new NetworkCredential(_emailSettings.Account, _emailSettings.Password);
            _smtpClient.EnableSsl = true;

            var message = new MailMessage()
            {
                From = new MailAddress(_emailSettings.Account),
                Subject = "Şifre Sıfırlama Linki",
                Body = @$"
                    <h4>Şifrenizi yenilemek için aşağıdaki linke tıklayınız.</h4>
                    <p><a href='{resetPasswordLink}'> Şifremi yenile</a></p>
                "
            };
            message.To.Add(to);
            message.IsBodyHtml = true;

            await _smtpClient.SendMailAsync(message);

        }
    }
}
