using FileService.Domain.Services;
using FileService.Infrastructure.Repositories;
using FileService.WebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Q.Commons.Helpers;
using Q.Infrastructure.Filters;

namespace FileService.WebAPI.Controllers
{
    [Route("api/[controller]/[cation]")]
    [ApiController]
    [UnitOfWork]
    public class UploadController(FSDomainService fSDomainService, FSRepository fSRepository) : ControllerBase
    {
        private readonly FSDomainService _fSDomainService = fSDomainService;
        private readonly FSRepository _fSRepository = fSRepository;

        /// <summary>
        /// 检查文件是否已存在
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> IsFileExsit(UploadRequest request)
        {
            var file = request.File;
            using var stream = file.OpenReadStream();
            var streamHash = HashHelper.ComputeSha256Hash(stream);
            return Ok(_fSRepository.FindAsync(stream.Length, streamHash) != null);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost]
        [RequestSizeLimit(60_000_000)]
        public async Task<ActionResult<Uri>> Upload(UploadRequest request, CancellationToken cancellationToken = default)
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
