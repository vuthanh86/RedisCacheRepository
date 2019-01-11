using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace RedisCache
{
    public interface IRedisCacheService : IDisposable
    {
        T Get<T>(string key);
        Task<T> GetAsync<T>(string key);
        T GetOrAdd<T>(string key, Func<string, T> factory);
        Task<T> GetOrAddAsync<T>(string key, Func<string, Task<T>> factory);
        void Put<T>(string key, T value);
        Task PutAsync<T>(string key, T value);
        void Remove(string key);
        Task RemoveAsync(string key);
    }

    public class RedisCacheService : IRedisCacheService
    {
        private readonly IRedisCaheFactory _factory;
        private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

        public RedisCacheService(IRedisCaheFactory factory)
        {
            _factory = factory;
        }

        public T Get<T>(string key)
        {
            var value = _factory.GetDatabase().StringGet(key);

            return value.HasValue ? Deserialize<T>(value) : default(T);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _factory.GetDatabase().StringGetAsync(key);

            return value.HasValue ? Deserialize<T>(value) : default(T);
        }

        public T GetOrAdd<T>(string key, Func<string, T> factory)
        {
            var value = _factory.GetDatabase().StringGet(key);

            if (!value.HasValue)
            {
                var result = factory(key);

                if (result != null)
                {
                    if (_factory.GetDatabase().StringSet(key, Serialize(result)))
                        return result;
                }
            }

            return default(T);
        }

        public async Task<T> GetOrAddAsync<T>(string key, Func<string, Task<T>> factory)
        {
            var value = await _factory.GetDatabase().StringGetAsync(key);

            if (!value.HasValue)
            {
                var result = await factory(key);

                if (result != null)
                {
                    if (await _factory.GetDatabase().StringSetAsync(key, Serialize(result)))
                        return result;
                }
            }

            return default(T);
        }

        public void Put<T>(string key, T value)
        {
            _factory.GetDatabase().StringSet(key, Serialize(value));
        }

        public Task PutAsync<T>(string key, T value)
        {
            return _factory.GetDatabase().StringSetAsync(key, Serialize(value));
        }

        public void Remove(string key)
        {
            _factory.GetDatabase().KeyDelete(key);
        }

        public Task RemoveAsync(string key)
        {
            return _factory.GetDatabase().KeyDeleteAsync(key);
        }

        private T Deserialize<T>(RedisValue value)
        {
            return JsonConvert.DeserializeObject<T>(value, serializerSettings);
        }

        private string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value, serializerSettings);
        }

        public void Dispose()
        {
        }
    }
}
