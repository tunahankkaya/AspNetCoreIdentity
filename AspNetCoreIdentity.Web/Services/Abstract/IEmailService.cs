namespace AspNetCoreIdentity.Web.Services.Abstract;

public interface IEmailService
{
    Task SendResetPasswordEmail(string resetPasswordEmailLink, string toEmail);
}