using ListeningService.Domain.Interfaces;
using ListeningService.Domain.ValueObjects;
using System.Text;

namespace ListeningService.Infrastructure.Services
{
    /// <summary>
    /// parser for *.srt files and *.vtt files
    /// </summary>
    class SrtParser : ISubtitleParser
    {
        public bool CanParse(string subtitleType)
        {
            return subtitleType.Equals("srt", StringComparison.OrdinalIgnoreCase)
                || subtitleType.Equals("vtt", StringComparison.OrdinalIgnoreCase);
        }

        public IEnumerable<Sentence> Parse(string subtitle)
        {
            var srtParser = new SubtitlesParser.Classes.Parsers.SubParser();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(subtitle)))
            {
                var items = srtParser.ParseStream(ms);
                return items.Select(s => new Sentence(TimeSpan.FromMilliseconds(s.StartTime),
                    TimeSpan.FromMilliseconds(s.EndTime), String.Join(" ", s.Lines)));
            }
        }
    }
}
