using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ListeningService.Infrastructure.Configs
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("ls_categories");
            builder.Property(e => e.CoverUrl).IsRequired(false).HasMaxLength(500).IsUnicode();
            builder.OwnsOne(e => e.Name, p =>
            {
                p.Property(c => c.Chinese).IsRequired(true).HasMaxLength(500).IsUnicode();
                p.Property(c => c.English).IsRequired(true).HasMaxLength(500).IsUnicode();
            });
        }
    }
}
