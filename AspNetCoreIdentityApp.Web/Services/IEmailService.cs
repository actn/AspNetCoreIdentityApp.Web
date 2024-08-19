namespace AspNetCoreIdentityApp.Web.Services
{
    public interface IEmailService
    {
        Task SendPasswordResetEmail(string resetPasswordLink, string to);
    }
}
