using CoinDeskAPI.Models.DataLayerModels.Currency;
using CoinDeskAPI.Models.ServiceModels.Currency;
using CoinDeskAPI.Provider.Currency;
using System.Globalization;
using System.Resources;

namespace CoinDeskAPI.Services.Currency;

public class CurrencyService : ICurrencyService
{
    private readonly ILogger<CurrencyService> _logger;
    private readonly ICurrencyProvider _currencyProvider;

    public CurrencyService(ILogger<CurrencyService> logger, ICurrencyProvider currencyProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _currencyProvider = currencyProvider ?? throw new ArgumentNullException(nameof(currencyProvider));
    }

    /// <summary>
    /// 新增幣別資料
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <param name="langKey">語系代碼</param>
    /// <param name="currencyName">幣別名稱</param>
    /// <param name="currencyShortName">幣別簡稱</param>
    /// <param name="currencyDescription">幣別說明</param>
    /// <returns>是否成功</returns>
    public async Task<bool> AddCurrencyDataAsync(string currencyCode, string langKey, string currencyName, string currencyShortName, string currencyDescription)
    {
        _logger.LogInformation("{0}-{1},{2},{3},{4},{5}", nameof(AddCurrencyDataAsync), currencyCode, langKey, currencyName, currencyShortName, currencyDescription);

        try
        {
            // 如果幣別資料已存在，則不新增；否則執行新增語系作業
            var currencyInfoExist = await _currencyProvider.QueryCurrencyInfoAsync(currencyCode).ConfigureAwait(false);

            int currencyInfoNewId = -1;

            var currencyInfo = new CurrencyInfo
            {
                CurrencyCode = currencyCode,
                LastModifiedTime = DateTime.Now,
                CurrencyLangs = new List<CurrencyLang>()
            };

            if (currencyInfoExist == null)
            {
                // 幣別資料不存在，新增幣別資訊
                currencyInfoNewId = await _currencyProvider.AddCurrencyInfoAsync(currencyInfo).ConfigureAwait(false);
            }

            var currencyLang = new CurrencyLang
            {
                CurrencyInfoId = currencyInfoNewId == -1 ? currencyInfoExist.CurrencyInfoId : currencyInfoNewId,
                LangKey = langKey,
                CurrencyName = currencyName,
                CurrencyShortName = currencyShortName,
                CurrencyDescription = currencyDescription,
                CurrencyInfo = currencyInfoNewId == -1 ? currencyInfoExist : currencyInfo
            };

            await _currencyProvider.AddCurrencyLangAsync(currencyLang).ConfigureAwait(false);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("{0}-{1}", nameof(AddCurrencyDataAsync), ex.Message);
            return false;
        }
    }

    /// <summary>
    /// 查詢幣別資料
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <returns>幣別資料集合</returns>
    public async Task<IEnumerable<CurrencyDataServiceModel>> QueryCurrencyDataAsync(string currencyCode)
    {
        try
        {
            _logger.LogInformation("{0}-{1}", nameof(QueryCurrencyDataAsync), currencyCode);

            var currencyDatas = await _currencyProvider.QueryCurrencyDatasAsync(currencyCode).ConfigureAwait(false);

            return currencyDatas.Select(cd => new CurrencyDataServiceModel
            {
                CurrencyInfoId = cd.CurrencyInfoId,
                CurrencyCode = cd.CurrencyCode,
                CurrencyName = cd.CurrencyName,
                CurrencyShortName = cd.CurrencyShortName,
                CurrencyDescription = cd.CurrencyDescription,
                CurrencyLangId = cd.CurrencyLangId,
                LangKey = cd.LangKey
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("{0}-{1}", nameof(QueryCurrencyDataAsync), ex.Message);
            return Enumerable.Empty<CurrencyDataServiceModel>();
        }
    }

    /// <summary>
    /// 查詢幣別名稱
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <param name="langKey">語系代碼</param>
    /// <returns>幣別名稱</returns>
    public async Task<string> QueryCurrencyNameAsync(string currencyCode, string langKey)
    {
        try
        {
            _logger.LogInformation("{0}-{1},{2}", nameof(QueryCurrencyNameAsync), currencyCode, langKey);

            var currencyName = await _currencyProvider.QueryCurrencyNameAsync(currencyCode, langKey).ConfigureAwait(false);

            // 查無幣別名稱時，回傳幣別代碼
            return string.IsNullOrEmpty(currencyName) ? currencyCode : currencyName;
        }
        catch (Exception ex)
        {
            _logger.LogError("{0}-{1}", nameof(QueryCurrencyNameAsync), ex.Message);
            return currencyCode;
        }
    }

    /// <summary>
    /// 查詢幣別(使用語系檔)
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <param name="langKey">語系代碼</param>
    /// <returns>幣別名稱</returns>
    public string GetCurrencyNameByResource(string currencyCode, string langKey)
    {
        try
        {
            _logger.LogInformation("{0}-{1},{2}", nameof(GetCurrencyNameByResource), currencyCode, langKey);

            var resourceManager = new ResourceManager($"{typeof(CurrencyService).Assembly.GetName().Name}.Resources.Currency", typeof(CurrencyService).Assembly);

            return resourceManager.GetString(currencyCode, CultureInfo.GetCultureInfo(langKey)) ?? currencyCode;
        }
        catch (Exception ex)
        {
            _logger.LogError("{0}-{1}", nameof(GetCurrencyNameByResource), ex.Message);
            return currencyCode;
        }
    }

    /// <summary>
    /// 查詢幣別資訊
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <param name="langKey">語系代碼</param>
    /// <returns>幣別資料服務模型</returns>
    public async Task<CurrencyDataServiceModel> QueryCurrencyInfoAsync(string currencyCode, string langKey)
    {
        try
        {
            _logger.LogInformation("{0}-{1},{2}", nameof(QueryCurrencyInfoAsync), currencyCode, langKey);

            var currencyData = await _currencyProvider.QueryCurrencyDataAsync(currencyCode, langKey).ConfigureAwait(false);

            if (currencyData == null)
            {
                return default(CurrencyDataServiceModel);
            }

            return new CurrencyDataServiceModel
            {
                CurrencyInfoId = currencyData.CurrencyInfoId,
                CurrencyCode = currencyData.CurrencyCode,
                CurrencyName = currencyData.CurrencyName,
                CurrencyShortName = currencyData.CurrencyShortName,
                CurrencyDescription = currencyData.CurrencyDescription,
                CurrencyLangId = currencyData.CurrencyLangId,
                LangKey = currencyData.LangKey
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("{0}-{1}", nameof(QueryCurrencyInfoAsync), ex.Message);
            return default(CurrencyDataServiceModel);
        }
    }

    /// <summary>
    /// 更新幣別名稱
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <param name="langKey">語系代碼</param>
    /// <param name="currencyName">幣別名稱</param>
    /// <returns>是否成功</returns>
    public async Task<bool> UpdateCurrencyNameAsync(string currencyCode, string langKey, string currencyName)
    {
        try
        {
            _logger.LogInformation("{0}-{1},{2},{3}", nameof(UpdateCurrencyNameAsync), currencyCode, langKey, currencyName);

            var currencyLang = await _currencyProvider.QueryCurrencyLangAsync(currencyCode, langKey).ConfigureAwait(false);

            if (currencyLang == null)
            {
                return false;
            }

            // 更新幣別資料
            currencyLang.CurrencyName = currencyName;
            currencyLang.CurrencyInfo.LastModifiedTime = DateTime.Now;

            return await _currencyProvider.UpdateCurrencyShortNameAsync(currencyLang).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError("{0}-{1}", nameof(UpdateCurrencyNameAsync), ex.Message);

            return false;
        }
    }

    /// <summary>
    /// 更新幣別簡稱
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <param name="langKey">語系代碼</param>
    /// <param name="currencyShortName">幣別簡稱</param>
    /// <returns>是否成功</returns>
    public async Task<bool> UpdateCurrencyShortNameAsync(string currencyCode, string langKey, string currencyShortName)
    {
        try
        {
            _logger.LogInformation("{0}-{1},{2},{3}", nameof(UpdateCurrencyShortNameAsync), currencyCode, langKey, currencyShortName);

            var currencyLang = await _currencyProvider.QueryCurrencyLangAsync(currencyCode, langKey).ConfigureAwait(false);

            if (currencyLang == null)
            {
                return false;
            }

            // 更新幣別資料
            currencyLang.CurrencyShortName = currencyShortName;
            currencyLang.CurrencyInfo.LastModifiedTime = DateTime.Now;

            return await _currencyProvider.UpdateCurrencyShortNameAsync(currencyLang).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError("{0}-{1}", nameof(UpdateCurrencyShortNameAsync), ex.Message);
            return false;
        }
    }

    /// <summary>
    /// 刪除幣別資料
    /// </summary>
    /// <param name="currencyCode">幣別代碼</param>
    /// <param name="langKey">語系代碼</param>
    /// <returns>是否成功</returns>
    public async Task<bool> DeleteCurrencyDataAsync(string currencyCode, string langKey)
    {
        try
        {
            _logger.LogInformation("{0}-{1},{2}", nameof(DeleteCurrencyDataAsync), currencyCode, langKey);

            return await _currencyProvider.DeleteCurrencyLangAsync(currencyCode, langKey).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError("{0}-{1}", nameof(DeleteCurrencyDataAsync), ex.Message);
            return false;
        }
    }
}