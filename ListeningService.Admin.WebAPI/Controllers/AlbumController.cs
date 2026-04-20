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
    public class AlbumController(IListeningRepository repository, ListeningDomainService domainService) : ControllerBase
    {
        private readonly IListeningRepository _repository = repository;
        private readonly ListeningDomainService _domainService = domainService;

        [HttpGet]
        public async Task<ActionResult<Album>> FindById(Guid albumId)
        {
            var album = await _repository.GetAlbumByIdAsync(albumId);
            if (album == null)
            {
                return NotFound();
            }
            return album;
        }

        [HttpGet]
        public async Task<ActionResult<Album[]>> FindAllByCategoryId(Guid categoryId)
        {
            var albums = await _repository.GetAlbumsByCategoryIdAsync(categoryId);
            return albums;
        }

        [HttpPost]
        public async Task<ActionResult<Album>> Add(AlbumAddRequest request)
        {
            var album = await _domainService.AddAlbumAsync(request.CategoryId, request.Name);
            return album;
        }

        [HttpPost]
        public async Task<ActionResult> Update(AlbumUpdateRequest request)
        {
            var album = await _repository.GetAlbumByIdAsync(request.AlbumId);
            if (album == null)
            {
                return NotFound();
            }
            album.ChangeName(request.Name);
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> DeleteById(Guid albumId)
        {
            var album = await _repository.GetAlbumByIdAsync(albumId);
            if (album == null)
            {
                return NotFound();
            }
            album.SoftDelete();
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> Show(Guid albumId)
        {
            var album = await _repository.GetAlbumByIdAsync(albumId);
            if (album == null)
            {
                return NotFound();
            }
            album.Show();
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> Hide(Guid albumId)
        {
            var album = await _repository.GetAlbumByIdAsync(albumId);
            if (album == null)
            {
                return NotFound();
            }
            album.Hide();
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> Sort(AlbumSortRequest request)
        {
            await _domainService.SortAlbumsAsync(request.CategoryId, request.AlbumIds);
            return Ok();
        }
    }
}
