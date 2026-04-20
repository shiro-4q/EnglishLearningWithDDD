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
    public class CategoryController(IListeningRepository repository, ICache cache) : ControllerBase
    {
        private readonly IListeningRepository _repository = repository;
        private readonly ICache _cache = cache;

        [HttpGet]
        public async Task<ActionResult<CategoryVM[]>> FindAll()
        {
            var categories = await _cache.GetOrCreateAsync("CategoryController.FindAll",
                async () => CategoryVM.Create(await _repository.GetCategoriesAsync()));
            return categories;
        }

        [HttpGet]
        public async Task<ActionResult<CategoryVM?>> FindById(Guid categoryId)
        {
            var category = await _cache.GetOrCreateAsync($"CategoryController.FindById.{categoryId}",
                async () => CategoryVM.Create(await _repository.GetCategoryByIdAsync(categoryId)));
            if (category == null)
                return NotFound();
            return category;
        }
    }
}
