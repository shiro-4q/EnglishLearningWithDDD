using IdentityService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace IdentityService.Infrastructure.Services
{
    public class MockEmailSender(ILogger<MockEmailSender> logger) : IEmailSender
    {
        public Task SendAsync(string email, string subject, string message)
        {
            logger.LogInformation("MockEmailSender: Sending email to {Email} with subject: {Subject} and message: {Message}", email, subject, message);
            return Task.CompletedTask;
        }
    }
}
