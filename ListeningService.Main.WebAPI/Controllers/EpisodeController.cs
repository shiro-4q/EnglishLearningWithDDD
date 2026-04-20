using ListeningService.Domain.Repositories;
using ListeningService.Domain.Services;
using ListeningService.Main.WebAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Q.Infrastructure.Cache;

namespace ListeningService.Main.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class EpisodeController(IListeningRepository repository, ICache cache, ListeningDomainService domainService) : ControllerBase
    {
        private readonly IListeningRepository _repository = repository;
        private readonly ICache _cache = cache;
        private readonly ListeningDomainService _domainService = domainService;

        [HttpGet]
        public async Task<ActionResult<EpisodeVM[]>> FindAllByAlbumId(Guid albumId)
        {
            // 写到单独的local函数的好处是避免回调中代码太复杂
            async Task<Episode[]> FindData()
            {
                var episodes = await _repository.GetVisibleEpisodesByAlbumIdAsync(albumId);
                foreach (var episode in episodes)
                {
                    episode.SetSentences(_domainService.ParseSubtitle(episode));
                }
                return episodes;
            }
            var episodes = await _cache.GetOrCreateAsync($"EpisodeController.FindAllByAlbumId.{albumId}",
                async () => EpisodeVM.Create(await FindData()));
            return episodes;
        }

        [HttpGet]
        public async Task<ActionResult<EpisodeVM?>> FindById(Guid episodeId)
        {
            async Task<Episode?> FindData()
            {
                var episode = await _repository.GetEpisodeByIdAsync(episodeId);
                if (episode == null)
                    return null;
                episode.SetSentences(_domainService.ParseSubtitle(episode));
                return episode;
            }
            var episode = await _cache.GetOrCreateAsync($"EpisodeController.FindById.{episodeId}",
                async () => EpisodeVM.Create(await FindData()));
            if (episode == null)
                return NotFound();
            return episode;
        }
    }
}
