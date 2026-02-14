using FluentValidation;
using Q.Infrastructure.Filters;

namespace IdentityService.WebAPI.DTOs
{
    [AutoValidation]
    public record ChangeMyPasswordRequest(string Password, string CheckPassword);

    public class ChangeMyPasswordRequestValidator : AbstractValidator<ChangeMyPasswordRequest>
    {
        public ChangeMyPasswordRequestValidator()
        {
            RuleFor(x => x.Password).NotEmpty().WithMessage("密码不能为空")
                .MinimumLength(6).WithMessage("密码程度不能小于6位");
            RuleFor(x => x.CheckPassword).NotEmpty().WithMessage("确认密码不能为空")
                .Equal(x => x.Password).WithMessage("两次输入的密码不一致");
        }
    }
}
