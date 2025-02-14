using System.ComponentModel.DataAnnotations.Schema;

namespace CoinDeskAPI.Models.DataLayerModels.Currency;

public class CurrencyInfo
{
    /// <summary>
    /// 幣別Id
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CurrencyInfoId { get; set; }

    /// <summary>
    /// 幣別代碼
    /// </summary>
    public required string CurrencyCode { get; set; }

    /// <summary>
    /// 最後更新時間
    /// </summary>
    public DateTime LastModifiedTime { get; set; }

    public ICollection<CurrencyLang> CurrencyLangs { get; set; }
}