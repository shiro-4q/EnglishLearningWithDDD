namespace MediaEncoderService.WebAPI.BgServices
{
    public class TranscodeBgServiceOptions
    {
        public int RestSeconds { get; set; } = 30;
        public string? WorkingDirectory { get; set; }
        public Uri? FileServiceUploadUrl { get; set; }
        public TimeSpan LockExpiry { get; set; } = TimeSpan.FromMinutes(30);
        public TimeSpan LockWait { get; set; } = TimeSpan.Zero;
        public TimeSpan LockRetry { get; set; } = TimeSpan.FromSeconds(1);
        public TimeSpan RestInterval => TimeSpan.FromSeconds(RestSeconds);
    }
}
