using System.Net;

namespace Application.Common;
public class OperationResult
{
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public string? ErrorCode { get; set; }
    public string? Message { get; set; }
    public IEnumerable<ResultError>? Errors { get; set; }
    public IDictionary<string, object>? MetaData { get; set; }

    public static OperationResult Success(
        int statusCode = (int)HttpStatusCode.OK,
        string? errorCode = null,
        IDictionary<string, object>? metaData = null)
    {
        return new OperationResult
        {
            IsSuccess = true,
            StatusCode = statusCode,
            ErrorCode = errorCode,
            MetaData = metaData
        };
    }

    public static OperationResult<TData> Success<TData>(
        TData data,
        int statusCode = (int)HttpStatusCode.OK,
        string? message = null,
        IDictionary<string, object>? metaData = null)
    {
        return new OperationResult<TData>
        {
            IsSuccess = true,
            StatusCode = statusCode,
            Message = message,
            MetaData = metaData,
            Data = data
        };
    }

    public static OperationResult Failure(
        int statusCode = (int)HttpStatusCode.InternalServerError,
        string? errorCode = null,
        string? message = null,
        IEnumerable<ResultError>? errors = null,
        IDictionary<string, object>? metaData = null)
    {
        return new OperationResult
        {
            IsSuccess = false,
            StatusCode = statusCode,
            ErrorCode = errorCode,
            Errors = errors,
            MetaData = metaData,
            Message = message
        };
    }
}

public class OperationResult<TData> : OperationResult
{
    public TData? Data { get; set; }
}