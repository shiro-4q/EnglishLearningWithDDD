using FluentValidation;
using Q.Infrastructure.Filters;

namespace FileService.WebAPI.DTOs
{
    [AutoValidation]
    public class UploadRequest
    {
        public IFormFile File { get; set; } = null!;
    }

    public class UploadRequestValidator : AbstractValidator<UploadRequest>
    {
        public UploadRequestValidator()
        {
            RuleFor(x => x.File).NotNull().WithMessage("文件不能为空");
            RuleFor(x => x.File.Length).GreaterThan(0).WithMessage("文件不能为空");
            RuleFor(x => x.File.Length).LessThanOrEqualTo(50_000_000).WithMessage("文件大小不能超过60MB");
        }
    }
}
