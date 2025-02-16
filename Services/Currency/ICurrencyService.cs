using CoinDeskAPI.Models.ServiceModels.Currency;

namespace CoinDeskAPI.Services.Currency;

/// <summary>
/// 幣別服務介面
/// </summary>
public interface ICurrencyService
{
    /// <summary>
    /// 新增幣別資料
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <param name="langKey">語系代碼</param>
    /// <param name="currencyName">幣別名稱</param>
    /// <param name="currencyShortName">幣別簡稱</param>
    /// <param name="currencyDescription">幣別說明</param>
    /// <returns>是否成功</returns>
    Task<bool> AddCurrencyDataAsync(string currencyCode, string langKey, string currencyName, string currencyShortName, string currencyDescription);

    /// <summary>
    /// 查詢幣別資料
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <returns>幣別資料集合</returns>
    Task<IEnumerable<CurrencyDataServiceModel>> QueryCurrencyDataAsync(string currencyCode);

    /// <summary>
    /// 查詢幣別資訊
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <param name="langKey">語系代碼</param>
    /// <returns>幣別資料服務模型</returns>
    Task<CurrencyDataServiceModel> QueryCurrencyInfoAsync(string currencyCode, string langKey);

    /// <summary>
    /// 查詢幣別名稱
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <param name="langKey">語系代碼</param>
    /// <returns>幣別名稱</returns>
    Task<string> QueryCurrencyNameAsync(string currencyCode, string langKey);

    /// <summary>
    /// 查詢幣別(使用語系檔)
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <param name="langKey">語系代碼</param>
    /// <returns>幣別名稱</returns>
    string GetCurrencyNameByResource(string currencyCode, string langKey);

    /// <summary>
    /// 更新幣別名稱
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <param name="langKey">語系代碼</param>
    /// <param name="currencyName">幣別名稱</param>
    /// <returns>是否成功</returns>
    Task<bool> UpdateCurrencyNameAsync(string currencyCode, string langKey, string currencyName);

    /// <summary>
    /// 更新幣別簡稱
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <param name="langKey">語系代碼</param>
    /// <param name="currencyShortName">幣別簡稱</param>
    /// <returns>是否成功</returns>
    Task<bool> UpdateCurrencyShortNameAsync(string currencyCode, string langKey, string currencyShortName);

    /// <summary>
    /// 刪除幣別資料
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <param name="langKey">語系代碼</param>
    /// <returns>是否成功</returns>
    Task<bool> DeleteCurrencyDataAsync(string currencyCode, string langKey);
}
