using System.Collections.Generic;
using System.Threading.Tasks;

namespace QForum.Web.Mailing
{
    public interface IMailSender
    {
        Task SendAsync(string subject, string body, List<Recipient> recipients = null);
    }
}