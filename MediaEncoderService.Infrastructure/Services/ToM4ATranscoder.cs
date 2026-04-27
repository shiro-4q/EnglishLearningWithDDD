using FFmpeg.NET;
using MediaEncoderService.Domain.Transcoder;

namespace MediaEncoder.Infrastructure;

public class ToM4ATranscoder : ITranscoder
{
    public bool CanTranscode(string format)
    {
        return "m4a".EqualsIgnoreCase(format);
    }

    public async Task TranscodeAsync(FileInfo sourceFile, FileInfo outputFile, string outputFormat, string[]? args, CancellationToken ct)
    {
        //可以用“FFmpeg.AutoGen”，因为他是bingding库，不用启动独立的进程，更靠谱。但是编程难度大，这里重点不是FFMPEG，所以先用命令行实现
        var input = new InputFile(sourceFile);
        var output = new OutputFile(outputFile);
        string baseDir = AppContext.BaseDirectory;//程序的运行根目录
        string ffmpegPath = Path.Combine(baseDir, "ffmpeg.exe");// 需要将ffmpeg.exe设置为“始终复制”到输出目录
        var ffmpeg = new Engine(ffmpegPath);
        string? errorMsg = null;
        ffmpeg.Error += (s, e) =>
        {
            errorMsg = e.Exception.Message;
        };
        await ffmpeg.ConvertAsync(input, output, ct);//进行转码
        if (errorMsg != null)
        {
            throw new Exception(errorMsg);
        }
    }
}
