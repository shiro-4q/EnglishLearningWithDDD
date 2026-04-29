using FluentValidation;
using Q.Infrastructure.Filters;

namespace SearchService.WebAPI.DTOs.Requests
{
    [AutoValidation]
    public record SearchEpisodeRequest(string Keyword, int PageIndex, int PageSize);

    public class SearchEpisodeRequestValidator : AbstractValidator<SearchEpisodeRequest>
    {
        public SearchEpisodeRequestValidator()
        {
            RuleFor(x => x.Keyword).NotEmpty().WithMessage("关键词不能为空");
            RuleFor(x => x.PageIndex).GreaterThan(0).WithMessage("页码必须大于0");
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("每页数量必须大于0");
        }
    }
}
