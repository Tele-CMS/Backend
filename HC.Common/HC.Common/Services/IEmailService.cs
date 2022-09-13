using HC.Common.Model.OrganizationSMTP;
using HC.Model;
using System.Threading.Tasks;

namespace HC.Common.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message, OrganizationSMTPCommonModel organizationSMTPModel, string organizationName);

        bool SendEmail(string email, string subject, string bodyHtml, OrganizationSMTPCommonModel organizationSMTPModel, string organizationName, string toEmail);

        Task<string> SendEmails(string email, string subject, string message, OrganizationSMTPCommonModel organizationSMTPModel, string organizationName);

    }
}
