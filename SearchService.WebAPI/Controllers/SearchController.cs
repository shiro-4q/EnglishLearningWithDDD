using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SearchService.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class SearchController(ISearchRepository repository) : ControllerBase
    {
        private readonly ISearchRepository _repository = repository;

        [HttpGet]
        public Task<SearchEpisodeResponse> SearchEpisodes(SearchEpisodeRequest request)
        {
            return _repository.SearchAsync(request.Keyword, request.PageIndex, request.PageSize);
        }
    }
}
