using System;
using System.Collections.Generic;
using System.Text;

namespace RedisCache
{
    public class CacheConfig
    {
        public TimeSpan Ttl { get; set; }

        public string Prefix { get; set; }
    }
}
