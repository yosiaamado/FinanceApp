namespace FinanceApp.Api.Model
{
    public class ApiResponse<T>
    {
        public string ResponseCode { get; set; } = "0";
        public T Data { get; set; }

        public ApiResponse(string code, T data)
        {
            ResponseCode = code;
            Data = data;
        }
        public static ApiResponse<T> Success(T data) => new("0", data);

        public static ApiResponse<string> CreatedSuccess() => new("0", "Data Created");

        public static ApiResponse<string> FailedMessage(string message, string code = "-1") => new(code, message);

        public static ApiResponse<T> Failed(T data, string code = "-1") => new(code, data);
    }

    public class Error
    {
        public string ErrorCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
