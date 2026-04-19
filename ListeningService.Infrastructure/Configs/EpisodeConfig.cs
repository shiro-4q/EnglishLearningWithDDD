using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ListeningService.Infrastructure.Configs
{
    public class EpisodeConfig : IEntityTypeConfiguration<Episode>
    {
        public void Configure(EntityTypeBuilder<Episode> builder)
        {
            builder.ToTable("ls_episodes");
            builder.OwnsOne(e => e.Name, p =>
            {
                p.Property(c => c.Chinese).IsRequired(true).HasMaxLength(500).IsUnicode();
                p.Property(c => c.English).IsRequired(true).HasMaxLength(500).IsUnicode();
            });
            builder.HasIndex(e => new { e.AlbumId, e.IsDeleted, e.IsVisible }); // 创建复合索引
        }
    }
}
