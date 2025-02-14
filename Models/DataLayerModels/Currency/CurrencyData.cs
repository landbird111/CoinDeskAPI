namespace CoinDeskAPI.Models.DataLayerModels.Currency;

/// <summary>
/// 幣別資料
/// </summary>
public class CurrencyData
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