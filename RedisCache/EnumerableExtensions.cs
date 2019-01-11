using System;
using System.Collections.Generic;
using System.Text;

namespace RedisCache
{
    public static class EnumerableExtensions
    {
        public static void RunBatchs<T>(this IEnumerable<T> values, int batchSize, Func<IEnumerable<T>> batchValues)
        {
            
        }
    }
}
