namespace Q.Infrastructure.Cache
{
    public interface ICache
    {
        TResult? GetOrCreate<TResult>(string cacheKey, Func<TResult?> valueFactory, int baseExpireSeconds = 60);
        Task<TResult?> GetOrCreateAsync<TResult>(string cacheKey, Func<Task<TResult?>> valueFactory, int baseExpireSeconds = 60);
        void Remove(string cacheKey);
        Task RemoveAsync(string cacheKey);
    }
}
