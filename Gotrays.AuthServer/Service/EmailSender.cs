using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Dantooine.Server.Service;

public class EmailSender :IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        await Task.CompletedTask;
    }
}