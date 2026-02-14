using IdentityService.Domain.Entities;

namespace IdentityService.WebAPI.DTOs
{
    public class UserResponse
    {
        public Guid Id { get; private set; }
        public string UserName { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;

        public static UserResponse Create(User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };
        }
    }
}
