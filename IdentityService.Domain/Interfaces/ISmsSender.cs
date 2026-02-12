namespace IdentityService.Domain.Interfaces
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}
