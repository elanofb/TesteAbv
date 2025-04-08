namespace Ambev.DeveloperEvaluation.WebApi.Common;

public class ApiResponseWithData<T> : ApiResponse
{
    // public ApiResponseWithData() {}
    // public ApiResponseWithData(bool success, string message, T data)
    //     : base(success, message)
    // {
    //     Data = data;
    // }
    public T? Data { get; set; }
}
