using CoinDeskAPI.Helper;
using CoinDeskAPI.Models.ViewModels;
using CoinDeskAPI.Models.ViewModels.Currency;
using CoinDeskAPI.Models.ViewModels.Prices;
using CoinDeskAPI.Provider.Currency;
using CoinDeskAPI.Services.Currency;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

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

// Monk���(��API�L�k�s�u�ɪ����N�ӷ�)
app = SetMonkDataPart(app);

// �I�s coindesk �� API�A�öi�����ഫ
app = CallCoindeskPart(app);

// �إ߹��O��CRUD API
app = CallCurrencyCRUDPart(app);

app.Run();

WebApplication CallCoindeskPart(WebApplication app)
{
    app.MapGet("/QueryCoindesk", async () =>
    {
        try
        {
            // API�s���W�ɪ��ƥΤ��
            var getCurrentPriceResponse = await httpClient.GetAsync(new Uri("http://localhost:5023/MonkGetBpiPriceData")).ConfigureAwait(false);
            // API�s�o�W��
            //var getCurrentPriceResponse = await httpClient.GetAsync(new Uri("https://api.coindesk.com/v1/bpi/currentprice.json")).ConfigureAwait(false);

            if (!getCurrentPriceResponse.IsSuccessStatusCode)
            {
                return new ApiResponseViewModel(isOk: false, message: "�I�sCoinDesk API����");
            }

            var content = await getCurrentPriceResponse.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(content))
            {
                return new ApiResponseViewModel(isOk: false, message: "�I�sCoinDesk API�^�������e����");
            }

            // �NJSON�r���ഫ������(CurrentPriceViewModel)
            var bpiCurrentPrice = JsonConvert.DeserializeObject<CurrentPriceViewModel>(content);

            if (bpiCurrentPrice == null)
            {
                return new ApiResponseViewModel(isOk: false, message: "�I�sCoinDesk API�^�������e�L�k�ഫ��CurrentPriceViewModel");
            }

            // �ഫ��s�ɶ��榡
            Func<DateTime?, string> ConvertUpdateTimeFormat = (DateTime? updateTime) => updateTime.HasValue ? updateTime.Value.ToString("yyyy/MM/dd HH:mm:ss") : string.Empty;

            // ��X��s�ɶ�
            var updateTimeSection = new
            {
                UpdateTime = ConvertUpdateTimeFormat(bpiCurrentPrice.TimeInfo.UpdateTimeUTCDateTime),
                UpdateTimeISO = ConvertUpdateTimeFormat(bpiCurrentPrice.TimeInfo.UpdatedTimeISODateTime),
                UpdateTimeUK = ConvertUpdateTimeFormat(bpiCurrentPrice.TimeInfo.UpdateTimeUTCDateTime)
            };

            // �N���C����Currency�ഫ��List
            var currencyInfos = bpiCurrentPrice.Bpi.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(CurrencyInformation))
                .Select(p => p.GetValue(bpiCurrentPrice.Bpi) as CurrencyInformation)
                .Select(s => new { CurrencyCode = s?.CurrencyCode, CurrencyRate = s?.CurrencyRate });

            return new ApiResponseViewModel<object>(isOk: true, data: new { UpdateTimes = updateTimeSection, CurrencyInfos = currencyInfos });
        }
        catch (Exception ex)
        {
            return new ApiResponseViewModel(isOk: false, message: $"�B�zAPI���e����, ��]: {ex.Message}");
        }
    })
    .WithName("QueryCoindesk")
    .WithOpenApi();

    return app;
}

WebApplication CallCurrencyCRUDPart(WebApplication app)
{
    // �d�߫��w�f�����W��
    app.MapGet("/QueryCurrencyName", async (ICurrencyService currencyService, string currencyCode) =>
    {
        // ���o�ثe�������Ҫ��y�t�W��
        var currencyName = await currencyService.QueryCurrencyNameAsync(currencyCode, CultureInfo.CurrentCulture.Name).ConfigureAwait(false);

        return new ApiResponseViewModel<string>(isOk: true, data: currencyName);
    }).WithName("QueryCurrencyName")
    .WithOpenApi();

    // ���o���O��ƲM��
    app.MapGet("/QueryCurrencyInfos", async (ICurrencyService currencyService, string currencyCode) =>
    {
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

    // ���o�ثe�y�t�����O���
    app.MapGet("/QueryCurrencyInfo", async (ICurrencyService currencyService, string currencyCode) =>
    {
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
        } : null, message: hasCurrencyInfo ? "�d�߹��O��Ʀ��\" : "�d�߹��O��ƥ���");
    }).WithName("QueryCurrencyInfo")
        .WithOpenApi();

    // �s�W���O���
    app.MapPost("/AddNewCurrencyData", async (ICurrencyService currencyService, [FromBody] AddCurrencyViewModel addCurrency) =>
    {
        var addCurrencyResult = await currencyService.AddCurrencyDataAsync(
             addCurrency.CurrencyCode,
            CultureInfo.CurrentCulture.Name,
            addCurrency.FullCurrencyName,
            addCurrency.ShortCurrencyName,
            addCurrency.CurrencyDesc).ConfigureAwait(false);

        return new ApiResponseViewModel(isOk: addCurrencyResult, message: addCurrencyResult ? "�s�W���O���\" : "�s�W���O����");
    }).WithName("AddNewCurrencyData")
        .WithOpenApi();

    app.MapPut("/UpdateCurrencyShortName", async (ICurrencyService currencyService, string currencyCode, string currencyShortName) =>
    {
        var updateShortNameResult = await currencyService.UpdateCurrencyShortNameAsync(currencyCode, CultureInfo.CurrentCulture.Name, currencyShortName).ConfigureAwait(false);

        return new ApiResponseViewModel(isOk: updateShortNameResult, message: updateShortNameResult ? "��s���O²�٦��\" : "��s���O²�٥���");
    }).WithName("UpdateCurrencyShortName")
        .WithOpenApi();

    app.MapDelete("/DeleteCurrencyCode", async (ICurrencyService currencyService, string currencyCode) =>
    {
        var deleteLangResult = await currencyService.DeleteCurrencyDataAsync(currencyCode, CultureInfo.CurrentCulture.Name).ConfigureAwait(false);

        return new ApiResponseViewModel(isOk: deleteLangResult, message: deleteLangResult ? "�R�����O���\" : "�R�����O����");
    }).WithName("DeleteCurrencyCode")
        .WithOpenApi();

    return app;
}

WebApplication SetMonkDataPart(WebApplication app)
{
    // api.coindesk.com/v1/bpi/currentprice.json���ĮɡA���Ѥ@�ӥ��a�ݪ�API
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