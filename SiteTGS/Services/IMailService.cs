using SiteTGS.Models;
using System.Threading.Tasks;

namespace SiteTGS.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
