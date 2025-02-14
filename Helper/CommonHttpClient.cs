namespace CoinDeskAPI.Helper
{
    /// <summary>
    /// 通用 HttpClient 呼叫作業。
    /// </summary>
    public class CommonHttpClient : ICommonHttpClient
    {
        /// <summary>
        /// 實現呼叫HttpClient的實例核心。
        /// </summary>
        private readonly IHttpClientCore _httpClient;

        /// <summary>
        /// 預設超時時間。
        /// </summary>
        private readonly double defaultTimeout = 30;

        public CommonHttpClient(IHttpClientCore httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// 發送一個 DELETE 請求到指定的 Uri。
        /// </summary>
        /// <param name="requestUri">請求發送到的 Uri。</param>
        /// <returns>回應訊息。</returns>
        public async Task<HttpResponseMessage> DeleteAsync(Uri requestUri)
        {
            return await _httpClient.DeleteAsync(requestUri).ConfigureAwait(false);
        }

        /// <summary>
        /// 發送一個 GET 請求到指定的 Uri，並附上給定的超時時間。
        /// </summary>
        /// <param name="requestUri">請求發送到的 Uri。</param>
        /// <returns>回應訊息。</returns>
        public async Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            return await _httpClient.GetAsync(requestUri, TimeSpan.FromSeconds(defaultTimeout)).ConfigureAwait(false);
        }

        /// <summary>
        /// 發送一個 POST 請求到指定的 Uri，並附上給定的內容。
        /// </summary>
        /// <param name="requestUri">請求發送到的 Uri。</param>
        /// <param name="content">請求的內容。</param>
        /// <returns>回應訊息。</returns>
        public async Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content)
        {
            return await _httpClient.PostAsync(requestUri, content, TimeSpan.FromSeconds(defaultTimeout)).ConfigureAwait(false);
        }

        /// <summary>
        /// 發送一個 PUT 請求到指定的 Uri，並附上給定的內容。
        /// </summary>
        /// <param name="requestUri">請求發送到的 Uri。</param>
        /// <param name="content">請求的內容。</param>
        /// <returns>回應訊息。</returns>
        public async Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content)
        {
            return await _httpClient.PutAsync(requestUri, content).ConfigureAwait(false);
        }
    }
}