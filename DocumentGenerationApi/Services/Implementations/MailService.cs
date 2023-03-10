using DocumentGenerationApi.Models.ResponseViewModels;
using DocumentGenerationApi.Services.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using System.Net.Mime;

namespace DocumentGenerationApi.Services.Implementations
{
    public class MailService : IMailService
    {
        private readonly ILogService _logService;
        public MailService(IServiceProvider serviceProvider) 
        {
            _logService = serviceProvider.GetRequiredService<ILogService>();
        }
        public async Task CreateMail(Byte[] pdf)
        {
            var email = new MimeMessage();
            email.Subject = "New Template Attached";
            email.From.Add(MailboxAddress.Parse("aspwebapimail@gmail.com"));
            email.To.Add(MailboxAddress.Parse("abhishekdavps@gmail.com"));

            var builder = new BodyBuilder();

            builder.TextBody = "Please find the attached file!";
            var stream = new MemoryStream(pdf);
            await builder.Attachments.AddAsync("Template", stream, MimeKit.ContentType.Parse(MediaTypeNames.Application.Pdf));
            email.Body = builder.ToMessageBody();

            await SendMailAsync(email);


        }

        private async Task SendMailAsync(MimeMessage mail)
        {
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, false);
            await smtp.AuthenticateAsync("aspwebapimail@gmail.com", "payyvzvrwspedhfb");
            var item = await smtp.SendAsync(mail);
            await smtp.DisconnectAsync(true);

        }
    }
}
