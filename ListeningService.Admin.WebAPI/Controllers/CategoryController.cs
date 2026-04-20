using ListeningService.Admin.WebAPI.DTOs.Requests;
using ListeningService.Domain.Repositories;
using ListeningService.Domain.Services;
using ListeningService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Q.Infrastructure.Filters;

namespace ListeningService.Admin.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [UnitOfWork(typeof(LSDbContext))]
    // 后台管理不用加缓存
    public class CategoryController(IListeningRepository repository, ListeningDomainService domainService) : ControllerBase
    {
        private readonly IListeningRepository _repository = repository;
        private readonly ListeningDomainService _domainService = domainService;

        [HttpGet]
        public async Task<ActionResult<Category>> FindById(Guid categoryId)
        {
            var category = await _repository.GetCategoryByIdAsync(categoryId);
            if (category == null)
            {
                return NotFound();
            }
            return category;
        }

        [HttpGet]
        public async Task<ActionResult<Category[]>> FindAll()
        {
            var categories = await _repository.GetCategoriesAsync();
            return categories;
        }

        [HttpPost]
        public async Task<ActionResult> Update(CategoryUpdateRequest request)
        {
            return Ok();
        }
    }
}
