using StackExchange.Redis;
using System.Text.Json;

namespace Q.Infrastructure.Cache
{
    public class RedisCache(IConnectionMultiplexer redisConn) : ICache
    {
        private readonly IDatabase _db = redisConn.GetDatabase();

        public TResult? GetOrCreate<TResult>(string cacheKey, Func<TResult?> valueFactory, int baseExpireSeconds = 60)
        {
            string value = _db.StringGet(cacheKey)!;
            if (string.IsNullOrEmpty(value))
            {
                var result = valueFactory();
                // null会被json序列化为字符串"null"，所以可以防范“缓存穿透”
                string jsonOfResult = JsonSerializer.Serialize(result);
                // 过期时间随机，防范“缓存雪崩”
                var expireSeconds = Random.Shared.Next(baseExpireSeconds, baseExpireSeconds * 2);
                _db.StringSet(cacheKey, jsonOfResult, TimeSpan.FromSeconds(expireSeconds));
                return result;
            }
            else
            {
                return JsonSerializer.Deserialize<TResult>(value);
            }
        }

        public async Task<TResult?> GetOrCreateAsync<TResult>(string cacheKey, Func<Task<TResult?>> valueFactory, int baseExpireSeconds = 60)
        {
            string? value = await _db.StringGetAsync(cacheKey);
            if (string.IsNullOrEmpty(value))
            {
                var result = await valueFactory();
                string jsonOfResult = JsonSerializer.Serialize(result);
                var expireSeconds = Random.Shared.Next(baseExpireSeconds, baseExpireSeconds * 2);
                await _db.StringSetAsync(cacheKey, jsonOfResult, TimeSpan.FromSeconds(expireSeconds));
                return result;
            }
            else
            {
                return JsonSerializer.Deserialize<TResult>(value);
            }
        }

        public void Remove(string cacheKey)
        {
            _db.KeyDelete(cacheKey);
        }

        public Task RemoveAsync(string cacheKey)
        {
            return _db.KeyDeleteAsync(cacheKey);
        }
    }
}
