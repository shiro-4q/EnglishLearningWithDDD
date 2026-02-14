using FluentValidation;
using Q.Infrastructure.Filters;

namespace IdentityService.WebAPI.DTOs
{
    [AutoValidation]
    public record LoginByUserNameAndPwdRequest(string UserName, string Password);

    public class LoginByUserNameAndPwdRequestValidator : AbstractValidator<LoginByUserNameAndPwdRequest>
    {
        public LoginByUserNameAndPwdRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("用户名不能为空");
            RuleFor(x => x.Password).NotEmpty().WithMessage("密码不能为空")
                .MinimumLength(6).WithMessage("密码程度不能小于6位");
        }
    }
}
