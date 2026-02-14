using FluentValidation;
using Q.Infrastructure.Filters;

namespace IdentityService.WebAPI.DTOs
{
    [AutoValidation]
    public record LoginByPhoneNumAndPwdRequest(string PhoneNum, string Password);

    public class LoginByPhoneNumAndPwdRequestValidator : AbstractValidator<LoginByPhoneNumAndPwdRequest>
    {
        public LoginByPhoneNumAndPwdRequestValidator()
        {
            RuleFor(x => x.PhoneNum).NotEmpty().WithMessage("手机号不能为空")
                .Matches(@"^1[3-9]\d{9}$").WithMessage("请输入有效的手机号");
            RuleFor(x => x.Password).NotEmpty().WithMessage("密码不能为空")
                .MinimumLength(6).WithMessage("密码程度不能小于6位");
        }
    }
}
