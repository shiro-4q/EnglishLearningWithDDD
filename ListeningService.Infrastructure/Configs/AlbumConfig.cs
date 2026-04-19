using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ListeningService.Infrastructure.Configs
{
    public class AlbumConfig : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder.ToTable("ls_albums");
            builder.OwnsOne(e => e.Name, p =>
            {
                p.Property(c => c.Chinese).IsRequired(true).HasMaxLength(500).IsUnicode();
                p.Property(c => c.English).IsRequired(true).HasMaxLength(500).IsUnicode();
            });
            builder.HasIndex(e => new { e.CategoryId, e.IsDeleted }); // 创建复合索引
        }
    }
}
