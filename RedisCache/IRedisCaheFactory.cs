using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;

namespace RedisCache
{
    public interface IRedisCaheFactory
    {
        IDatabase GetDatabase();
    }

    public class RedisCacheFactory : IRedisCaheFactory
    {
        private static ConfigurationOptions config; 
        public RedisCacheFactory(IOptions<ConfigurationOptions> cacheOptions)
        {
            config = cacheOptions.Value;
        }

        private readonly Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(config));

        public IDatabase GetDatabase()
        {
            return lazyConnection.Value.GetDatabase();
        }
    }
}
