namespace Application.Common.Interfaces;

public interface IEmailService
{
    public void SendEmail(string code, string email);
}