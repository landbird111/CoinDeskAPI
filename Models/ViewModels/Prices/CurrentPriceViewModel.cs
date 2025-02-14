using Newtonsoft.Json;
using System.ComponentModel;
using System.Globalization;

namespace CoinDeskAPI.Models.ViewModels.Prices;

/// <summary>
/// 表示來自 CoinDesk API 的當前價格資訊。
/// </summary>
[Description("BPI Current Price V1")]
public record CurrentPriceViewModel
{
    /// <summary>
    /// 獲取或設置時間資訊。
    /// </summary>
    [JsonProperty("time")]
    public required TimeInformation TimeInfo { get; set; }

    /// <summary>
    /// 獲取或設置免責聲明。
    /// </summary>
    [JsonProperty("disclaimer")]
    public string Disclaimer { get; set; } = string.Empty;

    /// <summary>
    /// 獲取或設置圖表名稱。
    /// </summary>
    [JsonProperty("chartName")]
    public required string ChartName { get; set; }

    /// <summary>
    /// 獲取或設置 BPI 資訊。
    /// <para>Business Process Improvement</para>
    /// </summary>
    [JsonProperty("bpi")]
    public Bpi Bpi { get; set; }
}

/// <summary>
/// 表示時間資訊。
/// </summary>
public class TimeInformation
{
    /// <summary>
    /// 獲取或設置 UTC 更新時間。
    /// </summary>
    [JsonProperty("updated")]
    public required string UpdateTimeUTC { get; set; }

    /// <summary>
    /// 以 DateTime 對象獲取 UTC 更新時間。
    /// </summary>
    public DateTime? UpdateTimeUTCDateTime => DateTime.TryParseExact(UpdateTimeUTC, "MMM d, yyyy HH:mm:ss 'UTC'", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result) ? result : default(DateTime);

    /// <summary>
    /// 獲取或設置 ISO 格式的更新時間。
    /// </summary>
    [JsonProperty("updatedISO")]
    public required string UpdatedTimeISO { get; set; }

    /// <summary>
    /// 以 DateTime 對象獲取 ISO 格式的更新時間。
    /// </summary>
    public DateTime? UpdatedTimeISODateTime => DateTime.TryParseExact(UpdatedTimeISO, "yyyy-MM-ddTHH:mm:ssK", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result) ? result : default(DateTime);

    /// <summary>
    /// 獲取或設置 GMT 更新時間。
    /// </summary>
    [JsonProperty("updateduk")]
    public required string UpdateTimeGMT { get; set; }

    /// <summary>
    /// 以 DateTime 對象獲取 GMT 更新時間。
    /// </summary>
    public DateTime? UpdateTimeGMTDateTime => DateTime.TryParseExact(UpdateTimeGMT, "MMM d, yyyy 'at' HH:mm 'GMT'", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result) ? result : default(DateTime);
}

/// <summary>
/// Business Process Improvement 資訊。
/// </summary>
public class Bpi
{
    /// <summary>
    /// 獲取或設置美元貨幣資訊。
    /// </summary>
    [JsonProperty("USD")]
    public CurrencyInformation USD { get; set; }

    /// <summary>
    /// 獲取或設置英鎊貨幣資訊。
    /// </summary>
    [JsonProperty("GBP")]
    public CurrencyInformation GBP { get; set; }

    /// <summary>
    /// 獲取或設置歐元貨幣資訊。
    /// </summary>
    [JsonProperty("EUR")]
    public CurrencyInformation EUR { get; set; }
}


/// <summary>
/// 表示貨幣資訊。
/// </summary>
public class CurrencyInformation
{
    /// <summary>
    /// 獲取或設置貨幣代碼。
    /// </summary>
    [JsonProperty("code")]
    public required string CurrencyCode { get; set; }

    /// <summary>
    /// 獲取或設置貨幣符號。
    /// </summary>
    [JsonProperty("symbol")]
    public required string CurrencySymbol { get; set; }

    /// <summary>
    /// 獲取或設置貨幣匯率。
    /// </summary>
    [JsonProperty("rate")]
    public required string CurrencyRate { get; set; }

    /// <summary>
    /// 獲取或設置貨幣描述。
    /// </summary>
    [JsonProperty("description")]
    public string CurrencyInfoDescription { get; set; } = string.Empty;

    /// <summary>
    /// 獲取或設置浮動匯率。
    /// </summary>
    [JsonProperty("rate_float")]
    public float CurrencyRateFloat { get; set; }
}


