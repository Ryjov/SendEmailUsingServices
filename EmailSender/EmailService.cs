using MimeKit;
using MailKit.Net.Smtp;

namespace EmailSender
{
    public class EmailService
    {
        public async Task SendTextMail(string to, string text)
        {
            try
            {
                using (var message = new MimeMessage())
                {
                    message.From.Add(new MailboxAddress("ERSender", "bolt321@mail.ru"));
                    message.To.Add(new MailboxAddress("", to));
                    message.Subject = "Testing";
                    message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = text
                    };

                    using (var client = new SmtpClient())
                    {
                        await client.ConnectAsync("smtp.yandex.ru", 25, false);
                        await client.AuthenticateAsync("login@yandex.ru", "password");
                        await client.SendAsync(message);

                        await client.DisconnectAsync(true);
                    }
                }
            }
            catch (Exception ex)
            {
                var s = ex.Message;
            }
        }
    }
}
