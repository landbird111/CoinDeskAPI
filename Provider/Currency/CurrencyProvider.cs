using CoinDeskAPI.Models.DataLayerModels.Currency;
using Microsoft.EntityFrameworkCore;

namespace CoinDeskAPI.Provider.Currency;

public class CurrencyProvider : ICurrencyProvider
{
    private readonly CurrencyContext _currencyContext;

    public CurrencyProvider(CurrencyContext currencyContext)
    {
        _currencyContext = currencyContext;
    }

    #region CurrencyInfo

    /// <summary>
    /// 新增幣別資訊
    /// </summary>
    /// <param name="currencyInfo">幣別資訊</param>
    /// <returns>新增的幣別資訊Id</returns>
    public async Task<int> AddCurrencyInfoAsync(CurrencyInfo currencyInfo)
    {
        await _currencyContext.CurrencyInfos.AddAsync(currencyInfo).ConfigureAwait(false);
        await _currencyContext.SaveChangesAsync().ConfigureAwait(false);
        return currencyInfo.CurrencyInfoId;
    }

    /// <summary>
    /// 更新幣別資訊
    /// </summary>
    /// <param name="currencyInfo">幣別資訊</param>
    /// <returns>是否更新成功</returns>
    public async Task<bool> UpdateCurrencyInfoAsync(CurrencyInfo currencyInfo)
    {
        _currencyContext.CurrencyInfos.Update(currencyInfo);
        return await _currencyContext.SaveChangesAsync().ConfigureAwait(false) > 0;
    }

    /// <summary>
    /// 查詢幣別資訊
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <returns>幣別資訊</returns>
    public async Task<CurrencyInfo> QueryCurrencyInfoAsync(string currencyCode)
    {
        return await _currencyContext.CurrencyInfos.FirstOrDefaultAsync(x => x.CurrencyCode == currencyCode).ConfigureAwait(false);
    }

    /// <summary>
    /// 刪除幣別資訊
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <returns>是否刪除成功</returns>
    public async Task<bool> DeleteCurrencyInfoAsync(string currencyCode)
    {
        var currencyInfo = await _currencyContext.CurrencyInfos.FirstOrDefaultAsync(x => x.CurrencyCode == currencyCode).ConfigureAwait(false);
        if (currencyInfo == null)
        {
            return false;
        }
        _currencyContext.CurrencyInfos.Remove(currencyInfo);
        return await _currencyContext.SaveChangesAsync().ConfigureAwait(false) > 0;
    }

    #endregion CurrencyInfo

    #region CurrencyLang

    /// <summary>
    /// 新增幣別語系
    /// </summary>
    /// <param name="currencyLang">幣別語系</param>
    /// <returns>是否新增成功</returns>
    public async Task AddCurrencyLangAsync(CurrencyLang currencyLang)
    {
        await _currencyContext.CurrencyLangs.AddAsync(currencyLang).ConfigureAwait(false);
    }

    /// <summary>
    /// 更新幣別簡稱
    /// </summary>
    /// <param name="currencyLang">幣別語系</param>
    /// <returns>是否更新成功</returns>
    public async Task<bool> UpdateCurrencyShortNameAsync(CurrencyLang currencyLang)
    {
        _currencyContext.CurrencyLangs.Update(currencyLang);
        return await _currencyContext.SaveChangesAsync().ConfigureAwait(false) > 0;
    }

    /// <summary>
    /// 查詢幣別語系
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <param name="langKey">語系代碼</param>
    /// <returns></returns>
    public async Task<CurrencyLang> QueryCurrencyLangAsync(string currencyCode, string langKey)
    {
        var currencyInfo = await _currencyContext.CurrencyInfos.FirstOrDefaultAsync(x => x.CurrencyCode == currencyCode).ConfigureAwait(false);

        if (currencyInfo == null)
        {
            return default(CurrencyLang);
        }

        return await _currencyContext.CurrencyLangs.FirstOrDefaultAsync(x => x.CurrencyInfoId == currencyInfo.CurrencyInfoId && x.LangKey == langKey).ConfigureAwait(false);
    }

    /// <summary>
    /// 查詢幣別簡稱
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <param name="langKey">語系代碼</param>
    /// <returns>幣別簡稱</returns>
    public async Task<string> QueryCurrencyShortNameAsync(string currencyCode, string langKey)
    {

        var currencyLang = await QueryCurrencyLangAsync(currencyCode, langKey).ConfigureAwait(false);

        return currencyLang?.CurrencyShortName ?? string.Empty;
    }

    /// <summary>
    /// 查詢幣別名稱
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <param name="langKey">語系代碼</param>
    /// <returns>幣別名稱</returns>
    public async Task<string> QueryCurrencyNameAsync(string currencyCode, string langKey)
    {
        var currencyLang = await QueryCurrencyLangAsync(currencyCode, langKey).ConfigureAwait(false);

        return currencyLang?.CurrencyName ?? string.Empty;
    }

    /// <summary>
    /// 刪除幣別語系
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <param name="langKey">語系代碼</param>
    /// <returns>是否刪除成功</returns>
    public async Task<bool> DeleteCurrencyLangAsync(string currencyCode, string langKey)
    {
        var currencyLang = await QueryCurrencyLangAsync(currencyCode, langKey).ConfigureAwait(false);
        if (currencyLang == null)
        {
            return false;
        }
        _currencyContext.CurrencyLangs.Remove(currencyLang);
        return await _currencyContext.SaveChangesAsync().ConfigureAwait(false) > 0;
    }

    #endregion CurrencyLang

    #region CurrencyData

    /// <summary>
    /// 查詢幣別資料
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <returns>幣別資料集合</returns>
    public async Task<IEnumerable<CurrencyData>> QueryCurrencyDatasAsync(string currencyCode)
    {
        var currencyData = from cd in _currencyContext.CurrencyInfos
                           join cl in _currencyContext.CurrencyLangs
                           on cd.CurrencyInfoId equals cl.CurrencyInfoId
                           where cd.CurrencyCode == currencyCode
                           select new CurrencyData
                           {
                               CurrencyInfoId = cd.CurrencyInfoId,
                               CurrencyCode = cd.CurrencyCode,
                               CurrencyName = cl.CurrencyName,
                               CurrencyShortName = cl.CurrencyShortName,
                               CurrencyDescription = cl.CurrencyDescription,
                               CurrencyLangId = cl.CurrencyLangId,
                               LangKey = cl.LangKey
                           };

        return await currencyData.ToListAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// 查詢幣別資料
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <param name="langKey">語系代碼</param>
    /// <returns>幣別資料</returns>
    public async Task<CurrencyData> QueryCurrencyDataAsync(string currencyCode, string langKey)
    {
        // 使用 LINQ 的 join 語法，將 CurrencyData 與 CurrencyLang 兩個資料表進行連接
        // 並且將 CurrencyData 與 CurrencyLang 兩個資料表的 CurrencyInfoId 欄位進行比對
        var currencyData = from cd in _currencyContext.CurrencyInfos
                           join cl in _currencyContext.CurrencyLangs
                           on cd.CurrencyInfoId equals cl.CurrencyInfoId
                           where cd.CurrencyCode == currencyCode && cl.LangKey == langKey
                           select new CurrencyData
                           {
                               CurrencyInfoId = cd.CurrencyInfoId,
                               CurrencyCode = cd.CurrencyCode,
                               CurrencyName = cl.CurrencyName,
                               CurrencyShortName = cl.CurrencyShortName,
                               CurrencyDescription = cl.CurrencyDescription,
                               CurrencyLangId = cl.CurrencyLangId,
                               LangKey = cl.LangKey
                           };

        return await currencyData.FirstOrDefaultAsync().ConfigureAwait(false);
    }

    #endregion CurrencyData
}