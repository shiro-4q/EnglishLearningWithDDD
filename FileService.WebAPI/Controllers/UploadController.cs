using FileService.Domain.Services;
using FileService.Infrastructure.Persistence;
using FileService.WebAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Q.Infrastructure.Filters;

namespace FileService.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UploadController(FSDomainService fSDomainService) : ControllerBase
    {
        private readonly FSDomainService _fSDomainService = fSDomainService;

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="request">上传文件请求DTO</param>
        /// <returns></returns>
        [HttpPost]
        [RequestSizeLimit(60_000_000)]
        [UnitOfWork(typeof(FSDbContext))]
        public async Task<ActionResult<Uri>> Upload([FromForm] UploadRequest request, CancellationToken cancellationToken = default)
        {
            var file = request.File;
            if (file == null || file.Length == 0)
                return BadRequest("文件不能为空");
            using var stream = file.OpenReadStream();
            var uploadItem = await _fSDomainService.UploadAsync(stream, file.FileName, cancellationToken);
            return uploadItem.RemoteUrl;
        }
    }
}
