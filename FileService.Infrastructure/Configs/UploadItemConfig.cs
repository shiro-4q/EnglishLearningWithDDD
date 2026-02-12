using FileService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileService.Infrastructure.Configs
{
    public class UploadItemConfig : IEntityTypeConfiguration<UploadItem>
    {
        public void Configure(EntityTypeBuilder<UploadItem> builder)
        {
            //因为使用UUIDv7作为主键，不会有数据库重排风险，所以不需要配置主键生成策略
            builder.ToTable("fs_upload_items");
            builder.Property(e => e.FileName).IsUnicode().HasMaxLength(1024);
            builder.Property(e => e.FileSHA256Hash).IsUnicode(false).HasMaxLength(64);
            builder.HasIndex(e => new { e.FileSizeInBytes, e.FileSHA256Hash });//经常要按照这两个列进行查询，因此把它们两个组成复合索引，提高查询效率
        }
    }
}
