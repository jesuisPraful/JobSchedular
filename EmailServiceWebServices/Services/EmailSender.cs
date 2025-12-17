using MailKit.Net.Smtp;
using MimeKit;

namespace EmailServiceWebServices.Services
{
    public class EmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendAsync(Models.Email email)
        {
            if (string.IsNullOrEmpty(email.ToEmail))
                throw new ArgumentException("Recipient email address is missing.");
            try
            {
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress("Notification System", _config["EmailSettings:SenderEmail"]));

                AddAddresses(message.To, email.ToEmail);
                AddAddresses(message.Cc, email.Cc);
                AddAddresses(message.Bcc, email.Bcc);

                message.Subject = email.Subject;
                message.Body = new BodyBuilder
                {
                    HtmlBody = email.Body
                }.ToMessageBody();

                using var smtp = new SmtpClient();

                int port = int.TryParse(_config["EmailSettings:SmtpPort"], out var p) ? p : 587;

                await smtp.ConnectAsync(
                    _config["EmailSettings:SmtpServer"],
                    port,
                    MailKit.Security.SecureSocketOptions.StartTls
                );

                await smtp.AuthenticateAsync(
                    _config["EmailSettings:SenderEmail"],
                    _config["EmailSettings:AppPassword"]
                );

                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Let BackgroundService handle retries & logging
                throw new InvalidOperationException(
                    $"Failed to send email to {email.ToEmail}", ex
                );
            }
        }

        private void AddAddresses(InternetAddressList list, string? emails)
        {
            if (string.IsNullOrWhiteSpace(emails)) return;

            foreach (var email in emails.Split(',', StringSplitOptions.RemoveEmptyEntries))
                list.Add(MailboxAddress.Parse(email.Trim()));
        }
    }
}
