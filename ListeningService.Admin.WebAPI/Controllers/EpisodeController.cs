using ListeningService.Admin.WebAPI.DTOs.Requests;
using ListeningService.Domain.Repositories;
using ListeningService.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Q.Infrastructure.Filters;

namespace ListeningService.Admin.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [UnitOfWork(typeof(LSDbContext))]
    public class EpisodeController(IListeningRepository repository, ListeningDomainService domainService) : ControllerBase
    {
        private readonly IListeningRepository _repository = repository;
        private readonly ListeningDomainService _domainService = domainService;

        [HttpGet]
        public async Task<ActionResult<Episode>> FindById(Guid episodeId)
        {
            var episode = await _repository.GetEpisodeByIdAsync(episodeId);
            if (episode == null)
            {
                return NotFound();
            }
            return episode;
        }

        [HttpGet]
        public async Task<ActionResult<Episode[]>> FindAllByAlbumId(Guid albumId)
        {
            var episodes = await _repository.GetEpisodesByAlbumIdAsync(albumId);
            return episodes;
        }

        [HttpPost]
        public async Task<ActionResult<Episode>> Add(EpisodeAddRequest request)
        {
            // todo：如果是m4a直接保存，如果是其他格式需要通知转码服务进行转码
            var episode = await _domainService.AddEpisodeAsync(request.AlbumId, request.Name, request.AudioUrl, request.DurationInSecond, request.Subtitle, request.SubtitleType);
            return episode;
        }

        [HttpPost]
        public async Task<ActionResult> Update(EpisodeUpdateRequest request)
        {
            var episode = await _repository.GetEpisodeByIdAsync(request.EpisodeId);
            if (episode == null)
            {
                return NotFound();
            }
            episode.ChangeName(request.Name);
            episode.ChangeSubtitle(request.Subtitle, request.SubtitleType);
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> DeleteById(Guid episodeId)
        {
            var episode = await _repository.GetEpisodeByIdAsync(episodeId);
            if (episode == null)
            {
                return NotFound();
            }
            episode.SoftDelete();
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> Show(Guid episodeId)
        {
            var episode = await _repository.GetEpisodeByIdAsync(episodeId);
            if (episode == null)
            {
                return NotFound();
            }
            episode.Show();
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> Hide(Guid episodeId)
        {
            var episode = await _repository.GetEpisodeByIdAsync(episodeId);
            if (episode == null)
            {
                return NotFound();
            }
            episode.Hide();
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> Sort(EpisodeSortRequest request)
        {
            await _domainService.SortEpisodesAsync(request.AlbumId, request.EpisodeIds);
            return Ok();
        }
    }
}
