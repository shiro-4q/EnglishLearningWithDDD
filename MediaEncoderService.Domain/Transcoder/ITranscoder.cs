namespace MediaEncoderService.Domain.Transcoder
{
    public interface ITranscoder
    {
        bool CanTranscode(string format);

        Task TranscodeAsync(FileInfo sourceFile, FileInfo outputFile, string outputFormat, string[]? args, CancellationToken ct);
    }
}
