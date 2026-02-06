namespace Q.Commons
{
    public static class BaseConfig
    {
        public static string ContentRootPath { get; private set; } = string.Empty;
        public static string WebRootPath { get; private set; } = string.Empty;

        public static void Init(string contentRootPath, string webRootPath)
        {
            ContentRootPath = contentRootPath;
            WebRootPath = webRootPath ?? Path.Combine(contentRootPath, "wwwroot");// 如果没启用wwwroot默认没有webRootPath，则手动组装webRootPath
        }
    }
}
