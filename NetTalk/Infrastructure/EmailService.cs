
using MimeKit;
using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using MimeKit.Text;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Infrastructure;

public class EmailService(IConfiguration configuration) : IEmailService
{
    public void SendEmail(string code, string email)
    {
        var emailMessage = GetConfirmMessage(code, email);
        using var client = new SmtpClient();
        try
        {
            client.Connect("smtp.gmail.com", 465, true);
            client.Authenticate("adm.nettalk@gmail.com", "pmch kswt zvlf vsiu");
            client.Send(emailMessage);
        }
        finally
        {
            client.Disconnect(true);
            client.Dispose();
            emailMessage.Dispose();
        }
    }

    private MimeMessage GetConfirmMessage(string code, string email)
    {
        const string name = "Администрация сайта";
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(name, "adm.nettalk@gmail.com"));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = name;
        emailMessage.Body = new TextPart(TextFormat.Html)
        {
            Text = code
        };
        return emailMessage;
    }
}