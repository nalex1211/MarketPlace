using MimeKit;
using static MarketPlace.Helpers.AllEmails;

namespace MarketPlace.Helpers;

public class EmailService
{
    public async Task SendEmailAsync(string email, string url, string subject)
    {
        MimeMessage message = new MimeMessage();
        message.Subject = subject;
        message.From.Add(new MailboxAddress("Confirmation", Emails.senderEmail));
        message.To.Add(new MailboxAddress("", email));
        message.Body = new BodyBuilder()
        {
            HtmlBody =
            "<div style=\"font-size:16px\">" +
            "Press the button below to confrim your account!" +
            "</div>" +
            "<div>" +
            $"<a href=\"{url}\" style=\"display: inline-block; padding: 16px 36px; font-family: 'Source Sans Pro' , Helvetica, Arial, sans-serif; font-size: 16px; color: white; text-decoration: none;-webkit-border-radius: 4px;\r\n    -moz-border-radius: 4px;\r\n    border-radius: 4px;\r\n    border: solid 1px black;\r\n background: #41B1FF;\r\n \">Confirm</a>" +
            "</div>"

        }.ToMessageBody();

        using (MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient())
        {
            await client.ConnectAsync("smtp.gmail.com", 465, true);
            await client.AuthenticateAsync(Emails.senderEmail, Emails.hostPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
