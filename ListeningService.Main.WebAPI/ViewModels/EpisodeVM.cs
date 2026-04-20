using ListeningService.Domain.ValueObjects;

namespace ListeningService.Main.WebAPI.ViewModels
{
    public record EpisodeVM(Guid Id, MultilingualString Name, Guid AlbumId, double DurationInSecond, Uri AudioUrl, IEnumerable<Sentence> Sentences)
    {
        public static EpisodeVM? Create(Episode? episode)
        {
            if (episode == null) return null;
            if (!episode.Sentences.Any()) throw new ArgumentException("Episode must have at least one sentence.");
            return new EpisodeVM(episode.Id, episode.Name, episode.AlbumId, episode.DurationInSecond, episode.AudioUrl, episode.Sentences);
        }

        public static EpisodeVM[] Create(IEnumerable<Episode> episodes)
        {
            return episodes.Select(a => Create(a)).ToArray()!;
        }
    }
}
