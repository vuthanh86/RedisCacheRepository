using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedisCache;
using StackExchange.Redis;

namespace RedisCacheTest
{
    [TestClass]
    public class FunctionTest
    {
        private IRedisCacheService Init()
        {
            var configuration = ConfigurationOptions.Parse("localhost,defaultDatabase=5,syncTimeout=10000");
            return new RedisCacheService(new RedisCacheFactory(new OptionsWrapper<ConfigurationOptions>(configuration)));
        }

        [TestMethod]
        public void Get_Test()
        {
            using (var cacheService = Init())
            {
                cacheService.Put("test", "vu thanh");
                var cacheValue = cacheService.Get<string>("test");

                Console.WriteLine($"Cache Result = {cacheValue}");
                Assert.AreEqual("vu thanh", cacheValue);

                cacheValue = cacheService.Get<string>("test 1");
                Assert.IsNull(cacheValue);

                cacheService.Remove("test");
            }
        }

        [TestMethod]
        public async Task GetAsync_Test()
        {
            using (var cacheService = Init())
            {
                cacheService.Put("test", "vu thanh");

                var cacheValue = await cacheService.GetAsync<string>("test");
                Console.WriteLine($"Cache Result = {cacheValue}");
                Assert.AreEqual("vu thanh", cacheValue);
                cacheService.Remove("test");
            }
        }

        [TestMethod]
        public async Task PutAll_Test()
        {
            using (var cacheService = Init())
            {
                var lists = Enumerable.Range(1, 100).Select(x => x.GetHashCode()).ToArray();

                cacheService.PutAll("all", lists);

                var results = cacheService.Get<int[]>("all");
                Assert.IsNotNull(results);
                Assert.AreEqual(100, results.Length);

                //cacheService.Remove("all");
            }
        }
    }
}
