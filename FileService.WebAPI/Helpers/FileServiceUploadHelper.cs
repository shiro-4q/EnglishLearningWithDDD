using Q.Swagger.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace FileService.WebAPI.Helpers
{
    public class FileServiceUploadHelper(IHttpClientFactory httpClientFactory, IJwtTokenService jwtTokenService)
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService;

        public async Task<Uri> UploadAsync(Uri uploadUrl, FileInfo file, CancellationToken ct = default)
        {
            var httpClient = _httpClientFactory.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, uploadUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", BuildAdminToken());

            await using var fileStream = file.OpenRead();
            using var content = new MultipartFormDataContent();
            content.Add(new StreamContent(fileStream), "File", file.Name);
            request.Content = content;

            using var response = await httpClient.SendAsync(request, ct);
            response.EnsureSuccessStatusCode();
            var uploadUrlText = await response.Content.ReadAsStringAsync(ct);
            return new Uri(uploadUrlText.Trim('"'));
        }

        private string BuildAdminToken()
        {
            Claim[] claims = [new Claim(ClaimTypes.Role, "Admin")];
            return _jwtTokenService.BuildToken(claims);
        }
    }
}
