namespace Q.Commons.Helpers
{
    public static class FileHelper
    {
        public static string GenerateFilePathWithHash(string hash, string fileName)
        {
            var today = DateTime.Today;
            return $"{today.Year}/{today.Month}/{today.Day}/{hash}/{fileName}";
        }
    }
}
