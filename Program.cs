using CoinDeskAPI.Helper;
using CoinDeskAPI.Models.ViewModels;
using CoinDeskAPI.Models.ViewModels.Currency;
using CoinDeskAPI.Models.ViewModels.Prices;
using CoinDeskAPI.Provider.Currency;
using CoinDeskAPI.Services.Currency;
using CoinDeskAPI.Validators.ViewModels.Currency;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddFluentValidation(config => config.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

// 增加多語系支援
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddHttpClient();

var httpClient = new CommonHttpClient(new HttpClientCore(builder.Services.BuildServiceProvider().GetRequiredService<IHttpClientFactory>()));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<CurrencyContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CoinDeskDBConnection"));
});

// Add logging
builder.Services.AddLogging();

// Register services as scoped
builder.Services.AddScoped<IValidator<AddCurrencyViewModel>, AddCurrencyVMValidator>();
builder.Services.AddScoped<ICurrencyProvider, CurrencyProvider>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Monk資料(當API無法連線時的替代來源)
app = SetMonkDataPart(app);

// 呼叫 coindesk 的 API，並進行資料轉換
app = CallCoindeskPart(app);

// 建立幣別的CRUD API
app = CallCurrencyCRUDPart(app);

app.Run();

WebApplication CallCoindeskPart(WebApplication app)
{
    app.MapGet("/QueryCoindesk", async (ICurrencyService currencyService) =>
    {
        try
        {
            // API連不上時的備用方案
            var getCurrentPriceResponse = await httpClient.GetAsync(new Uri("http://localhost:5023/MonkGetBpiPriceData")).ConfigureAwait(false);
            // API連得上時
            //var getCurrentPriceResponse = await httpClient.GetAsync(new Uri("https://api.coindesk.com/v1/bpi/currentprice.json")).ConfigureAwait(false);

            if (!getCurrentPriceResponse.IsSuccessStatusCode)
            {
                return new ApiResponseViewModel(isOk: false, message: "呼叫CoinDesk API失敗");
            }

            var content = await getCurrentPriceResponse.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(content))
            {
                return new ApiResponseViewModel(isOk: false, message: "呼叫CoinDesk API回應的內容為空");
            }

            // 將JSON字串轉換為物件(CurrentPriceViewModel)
            var bpiCurrentPrice = JsonConvert.DeserializeObject<CurrentPriceViewModel>(content);

            if (bpiCurrentPrice == null)
            {
                return new ApiResponseViewModel(isOk: false, message: "呼叫CoinDesk API回應的內容無法轉換為CurrentPriceViewModel");
            }

            // 轉換更新時間格式
            Func<DateTime?, string> ConvertUpdateTimeFormat = (DateTime? updateTime) => updateTime.HasValue ? updateTime.Value.ToString("yyyy/MM/dd HH:mm:ss") : string.Empty;

            // 輸出更新時間
            var updateTimeSection = new
            {
                UpdateTime = ConvertUpdateTimeFormat(bpiCurrentPrice.TimeInfo.UpdateTimeUTCDateTime),
                UpdateTimeISO = ConvertUpdateTimeFormat(bpiCurrentPrice.TimeInfo.UpdatedTimeISODateTime),
                UpdateTimeUK = ConvertUpdateTimeFormat(bpiCurrentPrice.TimeInfo.UpdateTimeUTCDateTime)
            };

            // 轉換幣別名稱，使用資料庫的方式
            Func<string, string> ConvertCurrencyNameByDB = (string currencyCode) => currencyService.QueryCurrencyNameAsync(currencyCode, CultureInfo.CurrentCulture.Name).GetAwaiter().GetResult();
            // 第二種: 多語系檔案的轉換方式
            Func<string, string> ConvertCurrencyNameByResx = (string currencyCode) => currencyService.GetCurrencyNameByResource(currencyCode, CultureInfo.CurrentCulture.Name);

            // 將條列式的Currency轉換為List(並依幣別代碼排序)
            var currencyInfos =
            bpiCurrentPrice.Bpi.GetType().GetProperties()
            .Where(p => p.PropertyType == typeof(CurrencyInformation))
            .Select(p => p.GetValue(bpiCurrentPrice.Bpi) as CurrencyInformation)
            .Select(s => new
            {
                CurrencyCode = s.CurrencyCode,
                // 取得幣別名稱的方式，最佳應透過快取機制，避免重複查詢
                //CurrencyName = ConvertCurrencyNameByDB(s.CurrencyCode),
                // 使用多語系檔案的轉換方式
                CurrencyName = ConvertCurrencyNameByResx(s.CurrencyCode),
                CurrencyRate = s.CurrencyRate
            })
            .OrderBy(x => x.CurrencyCode);

            return new ApiResponseViewModel<object>(isOk: true, data: new { UpdateTimes = updateTimeSection, CurrencyInfos = currencyInfos });
        }
        catch (Exception ex)
        {
            return new ApiResponseViewModel(isOk: false, message: $"處理API內容失敗, 原因: {ex.Message}");
        }
    })
    .WithName("QueryCoindesk")
    .WithOpenApi();

    return app;
}

WebApplication CallCurrencyCRUDPart(WebApplication app)
{
    // 查詢指定貨幣的名稱
    app.MapGet("/QueryCurrencyName", async (ICurrencyService currencyService, string currencyCode) =>
    {
        // 檢查貨幣代碼長度必須為3個字元
        if (currencyCode.Length != 3)
        {
            return new ApiResponseViewModel(isOk: false, message: "貨幣代碼長度必須為3個字元");
        }

        // 取得目前執行環境的語系名稱
        var currencyName = await currencyService.QueryCurrencyNameAsync(currencyCode, CultureInfo.CurrentCulture.Name).ConfigureAwait(false);

        return new ApiResponseViewModel<string>(isOk: true, data: currencyName);
    }).WithName("QueryCurrencyName")
    .WithOpenApi();

    // 取得幣別資料清單
    app.MapGet("/QueryCurrencyInfos", async (ICurrencyService currencyService, string currencyCode) =>
    {
        // 檢查貨幣代碼長度必須為3個字元
        if (currencyCode.Length != 3)
        {
            return new ApiResponseViewModel(isOk: false, message: "貨幣代碼長度必須為3個字元");
        }

        var currencyInfos = await currencyService.QueryCurrencyDataAsync(currencyCode).ConfigureAwait(false);

        return new ApiResponseViewModel<IEnumerable<CurrencyDataViewModel>>(isOk: currencyInfos.Any(), data: currencyInfos.Select(s => new CurrencyDataViewModel
        {
            CurrencyInfoId = s.CurrencyInfoId,
            CurrencyCode = s.CurrencyCode,
            CurrencyName = s.CurrencyName,
            CurrencyShortName = s.CurrencyShortName,
            CurrencyDescription = s.CurrencyDescription,
            CurrencyLangId = s.CurrencyLangId,
            LangKey = s.LangKey
        }));
    }).WithName("QueryCurrencyInfos")
        .WithOpenApi();

    // 取得目前語系的幣別資料
    app.MapGet("/QueryCurrencyInfo", async (ICurrencyService currencyService, string currencyCode) =>
    {
        // 檢查貨幣代碼長度必須為3個字元
        if (currencyCode.Length != 3)
        {
            return new ApiResponseViewModel(isOk: false, message: "貨幣代碼長度必須為3個字元");
        }

        var currencyInfo = await currencyService.QueryCurrencyInfoAsync(currencyCode, CultureInfo.CurrentCulture.Name).ConfigureAwait(false);

        var hasCurrencyInfo = currencyInfo != null;

        return new ApiResponseViewModel<CurrencyDataViewModel>(isOk: hasCurrencyInfo, data: hasCurrencyInfo ? new CurrencyDataViewModel
        {
            CurrencyInfoId = currencyInfo.CurrencyInfoId,
            CurrencyCode = currencyInfo.CurrencyCode,
            CurrencyName = currencyInfo.CurrencyName,
            CurrencyShortName = currencyInfo.CurrencyShortName,
            CurrencyDescription = currencyInfo.CurrencyDescription,
            CurrencyLangId = currencyInfo.CurrencyLangId,
            LangKey = currencyInfo.LangKey
        } : null, message: hasCurrencyInfo ? "查詢幣別資料成功" : "查詢幣別資料失敗");
    }).WithName("QueryCurrencyInfo")
        .WithOpenApi();

    // 新增幣別資料
    app.MapPost("/AddNewCurrencyData", async (ICurrencyService currencyService, IValidator<AddCurrencyViewModel> addCurrencyValidator, [FromBody] AddCurrencyViewModel addCurrency) =>
    {
        // 驗證新增幣別資料
        var validationResult = await addCurrencyValidator.ValidateAsync(addCurrency).ConfigureAwait(false);

        if (!validationResult.IsValid)
        {
            return new ApiResponseViewModel(isOk: false, message: string.Join(",", validationResult.Errors.Select(s => s.ErrorMessage)));
        }

        var addCurrencyResult = await currencyService.AddCurrencyDataAsync(
             addCurrency.CurrencyCode,
            CultureInfo.CurrentCulture.Name,
            addCurrency.FullCurrencyName,
            addCurrency.ShortCurrencyName,
            addCurrency.CurrencyDesc).ConfigureAwait(false);

        return new ApiResponseViewModel(isOk: addCurrencyResult, message: addCurrencyResult ? "新增幣別成功" : "新增幣別失敗");
    }).WithName("AddNewCurrencyData")
        .WithOpenApi();

    app.MapPut("/UpdateCurrencyShortName", async (ICurrencyService currencyService, string currencyCode, string currencyShortName) =>
    {
        // 檢查貨幣代碼長度必須為3個字元
        if (currencyCode.Length != 3)
        {
            return new ApiResponseViewModel(isOk: false, message: "貨幣代碼長度必須為3個字元");
        }

        var updateShortNameResult = await currencyService.UpdateCurrencyShortNameAsync(currencyCode, CultureInfo.CurrentCulture.Name, currencyShortName).ConfigureAwait(false);

        return new ApiResponseViewModel(isOk: updateShortNameResult, message: updateShortNameResult ? "更新幣別簡稱成功" : "更新幣別簡稱失敗");
    }).WithName("UpdateCurrencyShortName")
        .WithOpenApi();

    app.MapDelete("/DeleteCurrencyCode", async (ICurrencyService currencyService, string currencyCode) =>
    {
        // 檢查貨幣代碼長度必須為3個字元
        if (currencyCode.Length != 3)
        {
            return new ApiResponseViewModel(isOk: false, message: "貨幣代碼長度必須為3個字元");
        }

        var deleteLangResult = await currencyService.DeleteCurrencyDataAsync(currencyCode, CultureInfo.CurrentCulture.Name).ConfigureAwait(false);

        return new ApiResponseViewModel(isOk: deleteLangResult, message: deleteLangResult ? "刪除幣別成功" : "刪除幣別失敗");
    }).WithName("DeleteCurrencyCode")
        .WithOpenApi();

    return app;
}

WebApplication SetMonkDataPart(WebApplication app)
{
    // api.coindesk.com/v1/bpi/currentprice.json失效時，提供一個本地端的API
    app.MapGet("/MonkGetBpiPriceData", () =>
    {
        return Results.Text("""
        {
        time: {
        updated: "Feb 4, 2025 13:34:32 UTC",
        updatedISO: "2025-02-04T13:34:32+00:00",
        updateduk: "Feb 4, 2025 at 13:34 GMT"
        },
        disclaimer: "This data was produced from the CoinDesk Bitcoin Price Index (USD). Non-USD currency data converted using hourly conversion rate from openexchangerates.org",
        chartName: "Bitcoin",
        bpi: {
        USD: {
        code: "USD",
        symbol: "&#36;",
        rate: "100,695.327",
        description: "United States Dollar",
        rate_float: 100695.3269
        },
        GBP: {
        code: "GBP",
        symbol: "&pound;",
        rate: "80,867.108",
        description: "British Pound Sterling",
        rate_float: 80867.108
        },
        EUR: {
        code: "EUR",
        symbol: "&euro;",
        rate: "96,272.486",
        description: "Euro",
        rate_float: 96272.486
        }
        }
        }
        """);
    }).WithName("MonkGetBpiPriceData")
        .WithOpenApi();

    return app;
}