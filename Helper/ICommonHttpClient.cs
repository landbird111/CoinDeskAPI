namespace CoinDeskAPI.Helper;

public interface ICommonHttpClient
{
    /// <summary>
    /// 發送一個 DELETE 請求到指定的 Uri。
    /// </summary>
    /// <param name="requestUri">請求發送到的 Uri。</param>
    /// <returns>回應訊息。</returns>
    Task<HttpResponseMessage> DeleteAsync(Uri requestUri);

    /// <summary>
    /// 發送一個 GET 請求到指定的 Uri，並附上給定的超時時間。
    /// </summary>
    /// <param name="requestUri">請求發送到的 Uri。</param>
    /// <returns>回應訊息。</returns>
    Task<HttpResponseMessage> GetAsync(Uri requestUri);

    /// <summary>
    /// 發送一個 POST 請求到指定的 Uri，並附上給定的內容。
    /// </summary>
    /// <param name="requestUri">請求發送到的 Uri。</param>
    /// <param name="content">請求的內容。</param>
    /// <returns>回應訊息。</returns>
    Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content);

    /// <summary>
    /// 發送一個 PUT 請求到指定的 Uri，並附上給定的內容。
    /// </summary>
    /// <param name="requestUri">請求發送到的 Uri。</param>
    /// <param name="content">請求的內容。</param>
    /// <returns>回應訊息。</returns>
    Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content);
}