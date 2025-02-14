namespace CoinDeskAPI.Models.ViewModels;

public record ApiResponseViewModel
{
    /// <summary>
    /// 初始化 ApiResponseViewModel 類別的新執行個體。
    /// </summary>
    /// <param name="isOk">是否成功。</param>
    public ApiResponseViewModel(bool isOk = true)
    {
        IsOk = isOk;
    }

    /// <summary>
    /// 初始化 ApiResponseViewModel 類別的新執行個體。
    /// </summary>
    /// <param name="isOk">是否成功。</param>
    /// <param name="message">回應的訊息。</param>
    public ApiResponseViewModel(bool isOk, string message = "") : this(isOk)
    {
        Message = message;
    }

    /// <summary>
    /// 獲取或設置是否成功。
    /// </summary>
    public bool IsOk { get; private set; }

    /// <summary>
    /// 獲取或設置回應的訊息。
    /// </summary>
    public string? Message { get; set; }
}

public record ApiResponseViewModel<T> : ApiResponseViewModel
{
    /// <summary>
    /// 初始化 ApiResponseViewModel 類別的新執行個體。
    /// </summary>
    /// <param name="isOk">是否成功。</param>
    /// <param name="data">回應的資料。</param>
    /// <param name="message">回應的訊息。</param>
    public ApiResponseViewModel(bool isOk, T data, string message = "") : base(isOk, message)
    {
        this.Data = data;
    }

    /// <summary>
    /// 獲取或設置回應的資料。
    /// </summary>
    public T? Data { get; set; }
}