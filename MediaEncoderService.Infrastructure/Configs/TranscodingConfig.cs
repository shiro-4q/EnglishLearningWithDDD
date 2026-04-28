using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediaEncoderService.Infrastructure.Configs
{
    public class TranscodingConfig : IEntityTypeConfiguration<TranscodingItem>
    {
        public void Configure(EntityTypeBuilder<TranscodingItem> builder)
        {
            builder.ToTable("ts_transcoding_items");
            builder.Property(b => b.Name).HasMaxLength(100);
            builder.Property(b => b.FileSHA256Hash).HasMaxLength(64).IsUnicode(false);// 不使用Unicode编码，节省空间，查询效率略高，限制不能是中文
            builder.Property(b => b.OutputFormat).HasMaxLength(10).IsUnicode(false);
            builder.Property(b => b.Status).HasConversion<string>().HasMaxLength(20).IsUnicode(false);// 枚举转换为字符串存储，提高可读性
        }
    }
}
