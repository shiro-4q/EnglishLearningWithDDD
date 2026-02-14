using Microsoft.AspNetCore.Identity;
using Q.DomainCommons.Models;

namespace IdentityService.Domain.Entities
{
    public class User : IdentityUser<Guid>, ISoftDelete, IHasCreationTime, IHasDeletionTime
    {
        public bool IsDeleted { get; private set; }

        public DateTime CreationTime { get; init; }

        public DateTime? DeletionTime { get; private set; }

        public User(string userName) : base(userName)
        {
            Id = Guid.CreateVersion7();
            CreationTime = DateTime.Now;
        }

        public void SoftDelete()
        {
            IsDeleted = true;
            DeletionTime = DateTime.Now;
        }
    }
}
