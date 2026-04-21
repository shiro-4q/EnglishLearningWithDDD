using ListeningService.Domain.Entities;
using ListeningService.Domain.Interfaces;
using ListeningService.Domain.Repositories;
using ListeningService.Domain.ValueObjects;
using Q.DomainCommons.Models;

namespace ListeningService.Domain.Services
{
    public class ListeningDomainService(IListeningRepository repository, ISubtitleHelper subtitleHelper)
    {
        private readonly IListeningRepository _repository = repository;
        private readonly ISubtitleHelper _subtitleHelper = subtitleHelper;

        public async Task<Category> AddCategoryAsync(MultilingualString name, Uri converUrl)
        {
            var maxSeq = await _repository.GetMaxSeqOfCategoriesAsync();
            var category = new Category(name, maxSeq + 1, converUrl);
            await _repository.AddCategoryAsync(category);
            return category;
        }

        public async Task SortCategoriesAsync(Guid[] guids)
        {
            var categories = await _repository.GetCategoriesAsync();
            var ids = categories.Select(c => c.Id);
            if (!ids.SequenceEqual(guids))
                throw new Exception($"提交的待排序Id必须是所有的分类Id");

            int seqNum = 1;
            //todo:可以用一个in语言一次性查出来
            foreach (var guid in guids)
            {
                var category = await _repository.GetCategoryByIdAsync(guid) ?? throw new Exception($"CategoryId={guid} 不存在");
                category.ChangeSequenceNumber(seqNum++);
            }
        }

        public async Task<Album> AddAlbumAsync(Guid categoryId, MultilingualString name)
        {
            var maxSeq = await _repository.GetMaxSeqOfAlbumsAsync(categoryId);
            var album = new Album(name, maxSeq + 1, categoryId);
            await _repository.AddAlbumAsync(album);
            return album;
        }

        public async Task SortAlbumsAsync(Guid categoryId, Guid[] guids)
        {
            var albums = await _repository.GetAlbumsByCategoryIdAsync(categoryId);
            var ids = albums.Select(c => c.Id);
            if (!ids.SequenceEqual(guids))
                throw new Exception($"提交的待排序Id中必须是categoryId={categoryId}分类下所有的Id");

            int seqNum = 1;
            foreach (var guid in guids)
            {
                var album = await _repository.GetAlbumByIdAsync(guid) ?? throw new Exception($"AlbumId={guid} 不存在");
                album.ChangeSequenceNumber(seqNum++);
            }
        }

        public async Task<Episode> AddEpisodeAsync(Guid albumId, MultilingualString name, Uri audioUrl, double durationInSecond, string subtitle, string subtitleType)
        {
            if (!CanParseSubtitle(subtitleType))
                throw new Exception($"不支持{subtitleType}字幕类型");
            var maxSeq = await _repository.GetMaxSeqOfAlbumsAsync(albumId);
            var builder = new Episode.Builder();
            builder.SequenceNumber(maxSeq + 1).Name(name).AlbumId(albumId)
                .AudioUrl(audioUrl).DurationInSecond(durationInSecond)
                .Subtitle(subtitle).SubtitleType(subtitleType);
            var episode = builder.Build();
            await _repository.AddEpisodeAsync(episode);
            return episode;
        }

        public async Task SortEpisodesAsync(Guid albumId, Guid[] guids)
        {
            var episodes = await _repository.GetEpisodesByAlbumIdAsync(albumId);
            var ids = episodes.Select(c => c.Id);
            if (!ids.SequenceEqual(guids))
                throw new Exception($"提交的待排序Id中必须是albumId={albumId}专辑下所有的Id");

            int seqNum = 1;
            foreach (var guid in guids)
            {
                var episode = await _repository.GetEpisodeByIdAsync(guid) ?? throw new Exception($"EpisodeId={guid} 不存在");
                episode.ChangeSequenceNumber(seqNum++);
            }
        }

        public bool CanParseSubtitle(string subtitleType)
        {
            return _subtitleHelper.CanParse(subtitleType);
        }

        public void ChangeSubtitle(Episode episode, string subtitle, string subtitleType)
        {
            if (!CanParseSubtitle(subtitleType))
                throw new Exception($"不支持{subtitleType}字幕类型");
            episode.ChangeSubtitle(subtitle, subtitleType);
        }

        public IEnumerable<Sentence> ParseSubtitle(Episode episode)
        {
            if (!_subtitleHelper.CanParse(episode.SubtitleType))
                throw new Exception($"不支持{episode.SubtitleType}字幕类型");
            var parser = _subtitleHelper.GetParser(episode.SubtitleType);
            return parser.Parse(episode.Subtitle);
        }

    }
}
