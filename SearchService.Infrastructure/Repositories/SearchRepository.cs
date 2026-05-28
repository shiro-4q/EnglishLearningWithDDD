using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using SearchService.Domain.Repositories;
using SearchService.Domain.Responses;

namespace SearchService.Infrastructure.Repositories
{
    public class SearchRepository(ElasticsearchClient client) : ISearchRepository
    {
        private readonly ElasticsearchClient _client = client;

        public Task DeleteAsync(Guid id)
        {
            return _client.DeleteAsync(SearchIndices.Episode, id);
        }

        public async Task<SearchEpisodeResponse> SearchAsync(string keyword, int pageIndex, int pageSize)
        {
            var from = (pageIndex - 1) * pageSize;

            //// 高亮配置
            //var highlight = new Func<HighlightDescriptor<Episode>, IHighlight>(h => h
            //    .PreTags("<em>")
            //    .PostTags("</em>")
            //    .Fields(fs => fs
            //        .Field(f => f.PlainSubtitle)
            //    )
            //);

            SearchResponse<Episode> response = await _client.SearchAsync<Episode>(s => s
                .Indices(SearchIndices.Episode)
                .From(from)
                .Size(pageSize)
                .Query(q => q
                    .MultiMatch(m => m // 多字段匹配
                        .Fields(f => f.ChineseName, f => f.EnglishName, f => f.PlainSubtitle)
                        .Query(keyword)
                        .Type(TextQueryType.BestFields)// BestFields 多个字段中取最佳字段得分
                    )
                )
            );
            return new SearchEpisodeResponse(response.Documents, response.Total);
        }

        public Task UpsertAsync(Episode episode)
        {
            return _client.IndexAsync(episode, i => i.Index(SearchIndices.Episode).Id(episode.Id));
        }
    }
}
