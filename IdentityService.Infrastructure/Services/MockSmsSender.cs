using IdentityService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace IdentityService.Infrastructure.Services
{
    public class MockSmsSender(ILogger<MockSmsSender> logger) : ISmsSender
    {
        public Task SendAsync(string number, string message)
        {
            logger.LogInformation("MockSmsSender: Sending SMS to {Number} with message: {Message}", number, message);
            return Task.CompletedTask;
        }
    }
}
