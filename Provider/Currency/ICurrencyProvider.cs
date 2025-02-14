using CoinDeskAPI.Models.DataLayerModels.Currency;

namespace CoinDeskAPI.Provider.Currency;

public interface ICurrencyProvider
{
    #region CurrencyInfo

    /// <summary>
    /// 新增幣別資訊
    /// </summary>
    /// <param name="currencyInfo">幣別資訊</param>
    /// <returns>新增的幣別資訊Id</returns>
    Task<int> AddCurrencyInfoAsync(CurrencyInfo currencyInfo);

    /// <summary>
    /// 更新幣別資訊
    /// </summary>
    /// <param name="currencyInfo">幣別資訊</param>
    /// <returns>是否更新成功</returns>
    Task<bool> UpdateCurrencyInfoAsync(CurrencyInfo currencyInfo);

    /// <summary>
    /// 查詢幣別資訊
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <returns>幣別資訊</returns>
    Task<CurrencyInfo> QueryCurrencyInfoAsync(string currencyCode);

    /// <summary>
    /// 刪除幣別資訊
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <returns>是否刪除成功</returns>
    Task<bool> DeleteCurrencyInfoAsync(string currencyCode);

    #endregion CurrencyInfo

    #region CurrencyLang

    /// <summary>
    /// 新增幣別語系
    /// </summary>
    /// <param name="currencyLang">幣別語系</param>
    /// <returns></returns>
    Task AddCurrencyLangAsync(CurrencyLang currencyLang);

    /// <summary>
    /// 更新幣別簡稱
    /// </summary>
    /// <param name="currencyLang">幣別語系</param>
    /// <returns>是否更新成功</returns>
    Task<bool> UpdateCurrencyShortNameAsync(CurrencyLang currencyLang);

    /// <summary>
    /// 查詢幣別語系
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <param name="langKey">語系代碼</param>
    /// <returns></returns>
    Task<CurrencyLang> QueryCurrencyLangAsync(string currencyCode, string langKey);

    /// <summary>
    /// 查詢幣別簡稱
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <param name="langKey">語系代碼</param>
    /// <returns>幣別簡稱</returns>
    Task<string> QueryCurrencyShortNameAsync(string currencyCode, string langKey);

    /// <summary>
    /// 查詢幣別名稱
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <param name="langKey">語系代碼</param>
    /// <returns>幣別名稱</returns>
    Task<string> QueryCurrencyNameAsync(string currencyCode, string langKey);

    /// <summary>
    /// 刪除幣別語系
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <param name="langKey">語系代碼</param>
    /// <returns>是否刪除成功</returns>
    Task<bool> DeleteCurrencyLangAsync(string currencyCode, string langKey);

    #endregion CurrencyLang

    #region CurrencyData

    /// <summary>
    /// 查詢幣別資料
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <returns>幣別資料集合</returns>
    Task<IEnumerable<CurrencyData>> QueryCurrencyDatasAsync(string currencyCode);

    /// <summary>
    /// 查詢幣別資料
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <param name="langKey">語系代碼</param>
    /// <returns>幣別資料</returns>
    Task<CurrencyData> QueryCurrencyDataAsync(string currencyCode, string langKey);

    #endregion CurrencyData
}
