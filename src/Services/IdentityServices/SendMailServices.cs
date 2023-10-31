using System.Net;
using System.Net.Mail;

namespace IdentityServices.Authentication;

public class SendMailService
{

    /// <param name="to">Email to</param>
    /// <param name="guid">Genenrated Global User Identity </param> 
    public void SendEmailPasswordReset(string mailTo, string guid, int userID, string userName )
    {
        GetConfigurationData(out string fromUserName, out string password, out string clientLocation);
        SmtpClient smtpClient = new("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(fromUserName, password),
            EnableSsl = true,
        };
        MailMessage mailMessage = new(fromUserName!, mailTo)
        {
            IsBodyHtml = true,
            Subject = "Reset your password",
            Body = $"Click here to reset your password: <a href=\"{clientLocation}/{guid}\">{clientLocation}?guid={guid}&uid={userID}&username={userName}&email={mailTo}</a>"
        };
        smtpClient.Send(mailMessage);
        smtpClient.Dispose();
        mailMessage.Dispose();
    }
    void GetConfigurationData(out string fromUserName, out string password, out string clientLocation)
    {
        ConfigurationBuilder? builder = new();
        builder.AddJsonFile(Directory.GetCurrentDirectory() + "/appsettings.json");
        ConfigurationRoot? root = (ConfigurationRoot)builder.Build();
        fromUserName = root.GetSection("MailSettings").GetValue<string>("username")!;
        password = root.GetSection("MailSettings").GetValue<string>("password")!;
        clientLocation = root.GetValue<string>("ClientResetPassURL")!;
        builder = null;
        root = null;
    }
    public SendMailService()
    {
    }
}