using ListeningService.Domain.Interfaces;

namespace ListeningService.Infrastructure.Services
{
    public class SubtitleHelper(IEnumerable<ISubtitleParser> parsers) : ISubtitleHelper
    {
        private readonly IEnumerable<ISubtitleParser> _parsers = parsers;

        public bool CanParse(string subtitleType)
        {
            return _parsers.Any(x => x.CanParse(subtitleType));
        }

        public ISubtitleParser GetParser(string subtitleType)
        {
            return _parsers.First(x => x.CanParse(subtitleType));
        }
    }
}
