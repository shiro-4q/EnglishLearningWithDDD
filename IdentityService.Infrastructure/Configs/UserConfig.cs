using IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Infrastructure.Configs
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("ids_users");
            builder.HasQueryFilter(u => !u.IsDeleted); // 全局过滤器，自动过滤掉 IsDeleted = true 的记录
        }
    }
}
