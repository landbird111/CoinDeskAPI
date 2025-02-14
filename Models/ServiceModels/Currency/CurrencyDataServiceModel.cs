namespace CoinDeskAPI.Models.ServiceModels.Currency;

/// <summary>
/// 幣別資料服務模型
/// </summary>
public record CurrencyDataServiceModel
{
    /// <summary>
    /// 幣別語系Id
    /// </summary>
    public int CurrencyLangId { get; set; }

    /// <summary>
    /// 幣別Id
    /// </summary>
    public int CurrencyInfoId { get; set; }

    /// <summary>
    /// 幣別代碼
    /// </summary>
    public string CurrencyCode { get; set; }

    /// <summary>
    /// 幣別語系代碼
    /// </summary>
    public string LangKey { get; set; }

    /// <summary>
    /// 幣別語系名稱
    /// </summary>
    public string CurrencyName { get; set; }

    /// <summary>
    /// 幣別簡稱
    /// </summary>
    public string CurrencyShortName { get; set; }

    /// <summary>
    /// 幣別說明
    /// </summary>
    public string CurrencyDescription { get; set; }
}