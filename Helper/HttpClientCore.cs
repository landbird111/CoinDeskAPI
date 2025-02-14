namespace CoinDeskAPI.Helper;


public class HttpClientCore : IHttpClientCore
{
    private readonly IHttpClientFactory _httpClientFactory;

    // 添加標頭訊息到 HttpClient
    private void AddHeaderInfo(HttpClient client)
    {
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("User-Agent", "CoinDeskAPI");
    }

    // 構造函數，初始化 HttpClientFactory
    public HttpClientCore(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// 發送一個 DELETE 請求到指定的 Uri。
    /// </summary>
    /// <param name="requestUri">請求發送到的 Uri。</param>
    /// <returns>回應訊息。</returns>
    public async Task<HttpResponseMessage> DeleteAsync(Uri requestUri)
    {
        using HttpClient httpClient = _httpClientFactory.CreateClient();

        AddHeaderInfo(httpClient);

        return await httpClient.DeleteAsync(requestUri).ConfigureAwait(false);
    }

    /// <summary>
    /// 發送一個 GET 請求到指定的 Uri，並附上給定的超時時間。
    /// </summary>
    /// <param name="requestUri">請求發送到的 Uri。</param>
    /// <param name="timeout">請求的超時時間。</param>
    /// <returns>回應訊息。</returns>
    public async Task<HttpResponseMessage> GetAsync(Uri requestUri, TimeSpan timeout)
    {
        using HttpClient httpClient = _httpClientFactory.CreateClient();

        httpClient.Timeout = timeout;

        AddHeaderInfo(httpClient);

        return await httpClient.GetAsync(requestUri).ConfigureAwait(false);
    }

    /// <summary>
    /// 發送一個 POST 請求到指定的 Uri，並附上給定的內容。
    /// </summary>
    /// <param name="requestUri">請求發送到的 Uri。</param>
    /// <param name="content">請求的內容。</param>
    /// <returns>回應訊息。</returns>
    public async Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content)
    {
        using HttpClient httpClient = _httpClientFactory.CreateClient();

        AddHeaderInfo(httpClient);

        return await httpClient.PostAsync(requestUri, content).ConfigureAwait(false);
    }

    /// <summary>
    /// 發送一個 POST 請求到指定的 Uri，並附上給定的內容和超時時間。
    /// </summary>
    /// <param name="requestUri">請求發送到的 Uri。</param>
    /// <param name="content">請求的內容。</param>
    /// <param name="timeout">請求的超時時間。</param>
    /// <returns>回應訊息。</returns>
    public async Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, TimeSpan timeout)
    {
        using HttpClient httpClient = _httpClientFactory.CreateClient();

        httpClient.Timeout = timeout;

        AddHeaderInfo(httpClient);

        return await httpClient.PostAsync(requestUri, content).ConfigureAwait(false);
    }

    /// <summary>
    /// 發送一個 PUT 請求到指定的 Uri，並附上給定的內容。
    /// </summary>
    /// <param name="requestUri">請求發送到的 Uri。</param>
    /// <param name="content">請求的內容。</param>
    /// <returns>回應訊息。</returns>
    public async Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content)
    {
        using HttpClient httpClient = _httpClientFactory.CreateClient();

        AddHeaderInfo(httpClient);

        return await httpClient.PutAsync(requestUri, content).ConfigureAwait(false);
    }
}
