using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Test.Core.Exceptions;
using Test.Core.Services.Interfaces;
using Test.Core.Settings;

namespace Test.Core.Services
{
    internal sealed class CacheService<TKey, TValue> : ICacheService<TKey, TValue>
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<CacheService<TKey, TValue>> _logger;

        private readonly CacheSettingsOptions _settings;

        public CacheService(
            IMemoryCache cache,
            IOptions<CacheSettingsOptions> options,
            ILogger<CacheService<TKey, TValue>> logger)
        {
            _cache = cache;
            _settings = options.Value;
            _logger = logger;
        }

        public async Task<TValue> Get(TKey key, Func<Task<TValue>> action)
        {
            try
            {
                TValue value;
                if (!_cache.TryGetValue(key, out value))
                {
                    value = await action();
                    _cache.Set(key, value, new MemoryCacheEntryOptions().SetAbsoluteExpiration(_settings.ExpirationTime));
                }

                return value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new CoreException("Failed to get data from cache", ex);
            }
        }

        public async Task<List<TValue>> Get(TKey key, Func<Task<List<TValue>>> action)
        {
            try
            {
                List<TValue> value;
                if (!_cache.TryGetValue(key, out value))
                {
                    value = await action();
                    _cache.Set(key, value, new MemoryCacheEntryOptions().SetAbsoluteExpiration(_settings.ExpirationTime));
                }

                return value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new CoreException("Failed to get list data from cache", ex);
            }
        }
    }
}
