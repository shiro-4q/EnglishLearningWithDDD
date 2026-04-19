namespace ListeningService.Domain.Interfaces
{
    public interface ISubtitleHelper
    {
        bool CanParse(string subtitleType);
        ISubtitleParser GetParser(string subtitleType);
    }
}
