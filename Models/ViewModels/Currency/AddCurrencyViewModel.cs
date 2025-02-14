namespace CoinDeskAPI.Models.ViewModels.Currency
{
    /// <summary>
    /// 新增幣別ViewModel
    /// </summary>
    public class AddCurrencyViewModel
    {
        /// <summary>
        /// 貨幣代碼
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// 全名貨幣名稱
        /// </summary>
        public string FullCurrencyName { get; set; }

        /// <summary>
        /// 簡短貨幣名稱
        /// </summary>
        public string ShortCurrencyName { get; set; }

        /// <summary>
        /// 貨幣描述
        /// </summary>
        public string CurrencyDesc { get; set; }
    }
}