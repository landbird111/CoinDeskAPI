using CoinDeskAPI.Models.ServiceModels.Currency;
using Microsoft.Extensions.Caching.Memory;

namespace CoinDeskAPI.Helper;

/// <summary>
/// 快取工廠類別
/// </summary>
public class CacheFactory
{
    private readonly IMemoryCache _memoryCache;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromSeconds(30);

    /// <summary>
    /// 建構子，初始化快取工廠
    /// </summary>
    /// <param name="memoryCache">記憶體快取介面</param>
    public CacheFactory(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    /// <summary>
    /// 取得幣別資料到快取內
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <param name="cacheFactory">產生快取資料的工廠方法</param>
    /// <returns>幣別資料回應的資料模型</returns>
    public async Task<CurrencyDataServiceModel> CurrencyGetOrAddAsync(string currencyCode, Func<Task<CurrencyDataServiceModel>> cacheFactory)
    {
        if (!_memoryCache.TryGetValue(currencyCode, out CurrencyDataServiceModel cacheEntry))
        {
            // 如果快取不存在，則取得資料並加入快取
            cacheEntry = await cacheFactory();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(_cacheDuration);

            _memoryCache.Set(currencyCode, cacheEntry, cacheEntryOptions);
        }

        return cacheEntry;
    }
}
