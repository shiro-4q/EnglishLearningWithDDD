using ListeningService.Domain.Repositories;
using ListeningService.Main.WebAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Q.Infrastructure.Cache;

namespace ListeningService.Main.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AlbumController(IListeningRepository repository, ICache cache) : ControllerBase
    {
        private readonly IListeningRepository _repository = repository;
        private readonly ICache _cache = cache;

        [HttpGet]
        public async Task<ActionResult<AlbumVM[]>> FindAllByCategoryId(Guid categoryId)
        {
            var albums = await _cache.GetOrCreateAsync($"AlbumController.FindAllByCategoryId.{categoryId}",
                async () => AlbumVM.Create(await _repository.GetVisibleAlbumsByCategoryIdAsync(categoryId)));
            return albums;
        }

        [HttpGet]
        public async Task<ActionResult<AlbumVM?>> FindById(Guid albumId)
        {
            var album = await _cache.GetOrCreateAsync($"AlbumController.FindById.{albumId}",
                async () => AlbumVM.Create(await _repository.GetAlbumByIdAsync(albumId)));
            if (album == null)
                return NotFound();
            return album;
        }
    }
}
