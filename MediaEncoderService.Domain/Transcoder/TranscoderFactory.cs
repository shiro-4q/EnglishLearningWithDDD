namespace MediaEncoderService.Domain.Transcoder
{
    public class TranscoderFactory(IEnumerable<ITranscoder> transcoders)
    {
        private readonly IEnumerable<ITranscoder> _transcoders = transcoders;

        public bool CanTranscode(string format)
        {
            return _transcoders.Any(t => t.CanTranscode(format));
        }

        public Task TranscodeAsync(FileInfo sourceFile, FileInfo outputFile, string outputFormat, string[]? args, CancellationToken ct)
        {
            var transcoder = _transcoders.FirstOrDefault(t => t.CanTranscode(outputFormat)) ?? throw new NotSupportedException($"No transcoder found for format: {outputFormat}");
            return transcoder.TranscodeAsync(sourceFile, outputFile, outputFormat, args, ct);
        }
    }
}
