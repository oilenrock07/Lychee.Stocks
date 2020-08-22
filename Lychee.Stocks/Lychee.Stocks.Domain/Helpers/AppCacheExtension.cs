using LazyCache;

namespace Lychee.Stocks.Domain.Helpers
{
    public static class AppCacheExtension
    {
        public static void SafeRemove<T>(this IAppCache cache, string cacheKey)
        {
            if (cache.Get<T>(cacheKey) != null)
                cache.Remove(cacheKey);
        }

    }
}
